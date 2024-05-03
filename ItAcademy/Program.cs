
using ItAcademy.DAL;
using ItAcademy.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<AppUser, IdentityRole>(identityOptions =>
{
    identityOptions.User.RequireUniqueEmail = true;
    identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
    identityOptions.Password.RequiredLength = 8;
    identityOptions.Password.RequiredUniqueChars = 0;
    identityOptions.Password.RequireNonAlphanumeric = false;
    identityOptions.Lockout.AllowedForNewUsers = true;
    identityOptions.Lockout.MaxFailedAccessAttempts = 5;
    identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
     name: "default", 
     pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
