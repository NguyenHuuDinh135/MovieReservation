using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Server.Application;
using MovieReservation.Server.Infrastructure;
using MovieReservation.Server.Infrastructure.Services;
using MovieReservation.Server.Web.Middleware;
using StackExchange.Redis;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    //IgnoreCycles để tránh vòng lặp khi serialize
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        // Cấu hình để serialize enum dưới dạng string thay vì số
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        // Thêm dòng này để Swagger hiển thị enum dưới dạng string
        c.UseInlineDefinitionsForEnums();
    });

// // Cấu hình CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//     });
// });
// Gọi extension method đã tạo trong DependencyInjection.cs
builder.AddInfrastructureServices();
builder.AddApplicationServices();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// // Sử dụng CORS
// app.UseCors("AllowAll");

// Thêm middleware xử lý ngoại lệ
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

await app.InitialiseDatabaseAsync();
await app.Services.CreateScope().ServiceProvider
    .GetRequiredService<MovieReservationDbContextInitialiser>()
    .SeedAsync();

app.Run();
