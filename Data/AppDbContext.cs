using Microsoft.EntityFrameworkCore;
using SimpleLoginApi.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SimpleLoginApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Test için basit kullanıcılar
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, KullaniciAdi = "admin", Sifre = "123456", Email = "admin@test.com" },
            new User { Id = 2, KullaniciAdi = "test", Sifre = "test123", Email = "test@test.com" }
        );
    }
}
