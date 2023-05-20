using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using ClientMVCApartments.ViewModels.Auth;
using ClientMVCApartments.Models;
using System.Reflection;

namespace ClientMVCApartments.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var baseUrl = _configuration.GetConnectionString("AccountApiBaseUrl");
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistration model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/auth/register", model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                // Обработка ошибки регистрации
                var errorResponse = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", errorResponse);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin model)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var tokenModel = await response.Content.ReadFromJsonAsync<User>();

                // Проверка на null перед созданием утверждения
                if (model.UserName != null && tokenModel.Token != null)
                {
                    // Создание и сохранение аутентификационных куки
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, model.UserName),
                        new Claim("User", tokenModel.Token)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("MyAccount", "Account");
                }
            }
            else
            {
                // Обработка ошибки входа
                ModelState.AddModelError("", "Неправильное имя пользователя или пароль.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("List", "Apartments");
        }

        /*public IActionResult MyAccount()
        {
            var userName = User.Identity.Name;
            var token = HttpContext.Request.Cookies["AccessToken"]; // это нужно только в тех случаях, где нужен токен, а пока это не используется // получение значения токена из аутентификационных куки или другого источника
            var model = new User { Token = token, UserName = userName };
            ViewData["UserName"] = userName;
            return View(model);
        }*/
    }
}
