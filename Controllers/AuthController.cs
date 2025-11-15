using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleLoginApi.Data;
using SimpleLoginApi.Models;

namespace SimpleLoginApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.KullaniciAdi == request.KullaniciAdi && u.Sifre == request.Sifre);
        //FİRST OR DEFAULT LİSTEDE ŞARTI SAĞLAYAN İLK ELEMANI VERİR. EĞER BULAMAZSA NULL DEĞERİ DÖNDÜRÜR.
        //LİSTELERDE AYNI İŞİ YAPAN FİND METODU VAR

        if (user == null)
        {
            return NotFound(new LoginResponse
            {
                Success = false,
                Message = "Kullanıcı adı veya şifre hatalı!"
            });
        }

        return Ok(new LoginResponse
        {
            Success = true,
            Message = "Giriş başarılı!",
            KullaniciAdi = user.KullaniciAdi
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.KullaniciAdi == user.KullaniciAdi);

        if (existingUser != null)
        {
            return BadRequest(new LoginResponse
            {
                Success = false,
                Message = "Bu kullanıcı adı zaten alınmış!"
            });
        }

        var newUser = new User
        {
            KullaniciAdi = user.KullaniciAdi,
            Sifre = user.Sifre,
            Email = user.Email
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new LoginResponse
        {
            Success = true,
            Message = "Kayıt başarılı!",
            KullaniciAdi = newUser.KullaniciAdi
        });
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpGet("test")]
    public ActionResult<string> Test()
    {
        return Ok( new LoginResponse
        {   Success=true,
            Message="API ÇALIŞIYOR"
        });
    }

    [HttpPut("changepassword")]
    public async Task<ActionResult<LoginResponse>> ChangePasswordAsync(string kullaniciadi, string sifre, string yenisifre)
    {
        // FindAsync yerine FirstOrDefault kullan (kullanıcı adı ile arama)
        var foundUser = await _context.Users
            .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciadi);

        if (foundUser == null)
        {
            return Ok(new LoginResponse
            {
                Success = false,
                Message = "Kullanıcı bulunamadı!"
            });
        }

        if (foundUser.Sifre != sifre)
        {
            return Ok(new LoginResponse
            {
                Success = false,
                Message = "Şifre hatalı!"
            });
        }

        foundUser.Sifre = yenisifre;
        await _context.SaveChangesAsync();

        return Ok(new LoginResponse
        {
            Success = true,
            Message = "Şifre değiştirildi!",
            KullaniciAdi = foundUser.KullaniciAdi
        });
    }

    [HttpGet("getbyid")]
    public async Task<ActionResult<LoginResponse>> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user != null)
        {
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Kullanıcı bulundu!",
                KullaniciAdi = user.KullaniciAdi,
                Email = user.Email,
                Sifre = user.Sifre
            });
        }
        else
        {
            return Ok(new LoginResponse
            {
                Success = false,
                Message = "Kullanıcı bulunamadı!"
            });
        }
    }
}