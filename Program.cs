using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using SimpleLoginApi.Data;


var builder = WebApplication.CreateBuilder(args);

// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-Memory Database (Test için ideal)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDatabase"));

var app = builder.Build();

// Geliþtirme ortamýnda Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
}

// CORS (Frontend'den eriþim için)
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.MapControllers();

// Test verilerini yükle
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); // In-memory DB oluþtur
}

app.Run();