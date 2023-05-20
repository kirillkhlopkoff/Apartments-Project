using ClientMVCApartments.Controllers;
using ClientMVCApartments.Controllers.Account;
using ClientMVCApartments.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add configuration to the builder
builder.Configuration.AddJsonFile("appsettings.json", optional: true);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the HttpClient with the API base URL
builder.Services.AddHttpClient<ApartmentsController>()
    .ConfigureHttpClient((services, client) =>
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var baseUrl = configuration.GetConnectionString("ApiBaseUrl");
        client.BaseAddress = new Uri(baseUrl);
    });

// Регистрация HttpClient для контроллера аккаунта
builder.Services.AddHttpClient<AccountController>()
    .ConfigureHttpClient((services, client) =>
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var accountApiBaseUrl = configuration.GetConnectionString("AccountApiBaseUrl");
        client.BaseAddress = new Uri(accountApiBaseUrl);
    });

/*// Регистрация User и Role Manager
builder.Services.AddIdentity<MyAccountModel, IdentityRole>()
    .AddDefaultTokenProviders();*/

// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "YourCookieName";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Account/Login";
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

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
