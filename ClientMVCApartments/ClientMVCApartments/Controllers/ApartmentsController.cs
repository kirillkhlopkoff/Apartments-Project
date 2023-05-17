using ClientMVCApartments.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ClientMVCApartments.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ApartmentsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var baseUrl = _configuration.GetConnectionString("ApiBaseUrl");
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<IActionResult> List()
        {
            var response = await _httpClient.GetAsync("api/apartments");
            response.EnsureSuccessStatusCode();

            var apartments = await response.Content.ReadFromJsonAsync<Apartment[]>();

            return View(apartments);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Apartment apartment)
        {
            var userName = User.Identity.Name; // Получение имени пользователя из аутентификационных данных

            apartment.Owner = userName; // Присваивание имени пользователя полю Owner
            apartment.Status = "Active"; // Устанавливаем значение "Active" для свойства Status

            var response = await _httpClient.PostAsJsonAsync("api/apartments", apartment);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");

                return RedirectToAction(nameof(List));
            }

            return RedirectToAction(nameof(List));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UnActivate(int id)
        {
            var response = await _httpClient.PostAsync($"api/apartments/inactive/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(List));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");

                return RedirectToAction(nameof(List));
            }
        }

        /*[HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseAdress}api/apartments/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(List));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");

                return RedirectToAction(nameof(List));
            }
        }*/

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/apartments/{id}");

            if (response.IsSuccessStatusCode)
            {
                var apartment = await response.Content.ReadFromJsonAsync<Apartment>();
                return View(apartment);
            }

            return RedirectToAction(nameof(List));
        }

        //Поиск по названию
        public async Task<IActionResult> Search(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                var response = await _httpClient.GetAsync("api/apartments");
                response.EnsureSuccessStatusCode();

                var apartments = await response.Content.ReadFromJsonAsync<Apartment[]>();

                return View("List", apartments);
            }
            else
            {
                var response = await _httpClient.GetAsync($"api/apartments/search?title={title}");
                response.EnsureSuccessStatusCode();

                var apartments = await response.Content.ReadFromJsonAsync<Apartment[]>();

                return View("List", apartments);
            }
        }
    }
}
