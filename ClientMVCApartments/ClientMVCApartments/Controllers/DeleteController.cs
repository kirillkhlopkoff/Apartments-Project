using Microsoft.AspNetCore.Mvc;

namespace ClientMVCApartments.Controllers
{
    public class DeleteController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public DeleteController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var baseUrl = _configuration.GetConnectionString("ApiBaseUrl");
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.PostAsync($"api/delete/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Apartments");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorContent}");

                return RedirectToAction("List", "Apartments");
            }
        }
    }
}
