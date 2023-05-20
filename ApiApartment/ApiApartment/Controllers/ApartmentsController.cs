using ApiApartment.Context;
using ApiApartment.Models;
using ApiApartmentIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ApiApartment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Apartment>> SearchApartments(string title)
        {
            var apartments = _context.Apartments.Where(a => a.Title.Contains(title)).ToList();

            if (apartments == null)
            {
                return NotFound();
            }

            return apartments;
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public ActionResult<Apartment> GetApartment(int id)
        {
            var apartment = _context.Apartments.FirstOrDefault(a => a.Id == id);

            if (apartment == null)
            {
                return NotFound();
            }

            return apartment;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Apartment>> GetApartments()
        {
            var apartments = _context.Apartments.ToList();

            if (apartments == null)
            {
                return NotFound();
            }

            return apartments;
        }

        [HttpPost]
        public async Task<ActionResult<Apartment>> CreateApartment(Apartment apartment)
        {
            /*apartment.Owner = HttpContext.User.Identity.Name;
            apartment.UserId = HttpContext.User.Identity.Id;*/
            /*apartment.Owner = User.Identity.Name; // Заполняем поле "имя пользователя" значением текущего пользователя*/
            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApartment), new { id = apartment.Id }, apartment);
        }

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        [HttpPut("{id}")]
        public async Task<IActionResult> UnactivateApartment(int id, Apartment apartment)
        {
            apartment = await _context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            apartment.Status = "Inactive";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
