using ClientMVCApartments.Helpers;
using ClientMVCApartments.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;

namespace ClientMVCApartments.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly UserHelper _userHelper;

        public AccountController(HttpClient httpClient, IConfiguration configuration, UserHelper userHelper)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _userHelper = userHelper;

            var baseUrl = _configuration.GetConnectionString("AccountApiBaseUrl");
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public IActionResult MyAccount()
        {
            var userName = User.Identity.Name;
            var token = HttpContext.Request.Cookies["MyCookieName"];
            if (userName != null)
            {
                var user = _userHelper.GetUser(userName);
                if (user != null)
                {
                    user.Token = token;
                    ViewData["UserName"] = userName;
                    return View(user);
                }
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var userName = User.Identity.Name;
            var user = _userHelper.GetUser(userName);

            if (user == null)
            {
                // Обработайте ситуацию, если текущий пользователь не найден
                return RedirectToAction("Login", "Auth");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult Update(User model)
        {
            var userName = User.Identity.Name;
            var user = _userHelper.GetUser(userName);

            if (user == null)
            {
                // Обработайте ситуацию, если текущий пользователь не найден
                return RedirectToAction("Login", "Auth");
            }

            // Проверьте, заполнено ли поле FirstName и обновите информацию пользователя, если заполнено
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                user.City = model.City;
            }

            // Обновите информацию пользователя в источнике данных (например, в куки или другом месте)

            // В данном примере предполагается, что данные пользователя хранятся в JSON формате в куке с именем "UserData"
            var userDataJson = JsonSerializer.Serialize(user);
            HttpContext.Response.Cookies.Append("UserData", userDataJson);

            return RedirectToAction("MyAccount");
        }
    }
}
