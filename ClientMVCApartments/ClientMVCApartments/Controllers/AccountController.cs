using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using ClientMVCApartments.Models.Account;

namespace ClientMVCApartments.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        Uri baseAdress = new Uri("https://localhost:7295");        //апи идентификации
        /*private readonly string _apiBaseUrl;*/

        public AccountController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAdress;
            /*_apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");*/
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistration model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseAdress}api/auth/register", model);
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
            var response = await _httpClient.PostAsJsonAsync($"{baseAdress}api/auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<string>(tokenResponse);

                // Создание и сохранение аутентификационных куки
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim("AccessToken", token)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("List", "Apartments");
            }
            else
            {
                // Обработка ошибки входа
                ModelState.AddModelError("", "Неправильное имя пользователя или пароль.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("List", "Apartments");
        }
    }
}
