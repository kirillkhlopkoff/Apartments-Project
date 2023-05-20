using ClientMVCApartments.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace ClientMVCApartments.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var baseUrl = _configuration.GetConnectionString("AccountApiBaseUrl");
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public IActionResult MyAccount()
        {
            var userName = User.Identity.Name;
            var token = HttpContext.Request.Cookies["AccessToken"];
            if (userName != null)
            {
                var model = new User { Token = token, UserName = userName };
                ViewData["UserName"] = userName;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFirstName(User user)
        {
            var response = await _httpClient.PutAsJsonAsync("api/Account/FirstName", user);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyAccount");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLastName(User user)
        {
            var response = await _httpClient.PutAsJsonAsync("api/Account/LastName", user);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyAccount");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCity(User user)
        {
            var response = await _httpClient.PutAsJsonAsync("api/Account/City", user);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyAccount");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
