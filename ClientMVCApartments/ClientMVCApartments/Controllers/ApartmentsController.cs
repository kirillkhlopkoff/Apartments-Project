using ClientMVCApartments.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientMVCApartments.Controllers
{
    public class ApartmentsController : Controller
    {

        Uri baseAdress = new Uri("https://localhost:7298/");          //апи квартир
        private readonly HttpClient _httpClient;
        /*private readonly string _baseUrl;*/

        public ApartmentsController(HttpClient httpClient/*, IConfiguration configuration*/)
        {
            _httpClient = httpClient;
            /*_baseUrl = configuration.GetValue<string>("ApiBaseUrl");*/
            _httpClient.BaseAddress = baseAdress;
        }

        public async Task<IActionResult> List()
        {
            var response = await _httpClient.GetAsync($"{baseAdress}api/apartments");
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
            var response = await _httpClient.PostAsJsonAsync($"{baseAdress}api/apartments", apartment);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseAdress}api/apartments/{id}");
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(List));
        }

        //Поиск по названию
        public async Task<IActionResult> Search(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                var response = await _httpClient.GetAsync($"{baseAdress}api/apartments");
                response.EnsureSuccessStatusCode();

                var apartments = await response.Content.ReadFromJsonAsync<Apartment[]>();

                return View("List", apartments);
            }
            else
            {
                var response = await _httpClient.GetAsync($"{baseAdress}api/apartments/search?title={title}");
                response.EnsureSuccessStatusCode();

                var apartments = await response.Content.ReadFromJsonAsync<Apartment[]>();

                return View("List", apartments);
            }
        }
    }
}
