using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Server.Application;
using MovieReservation.Server.Infrastructure;
using MovieReservation.Server.Infrastructure.Services;
using StackExchange.Redis;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

await app.InitialiseDatabaseAsync();
await app.Services.CreateScope().ServiceProvider
    .GetRequiredService<MovieReservationDbContextInitialiser>()
    .SeedAsync();

app.Run();
