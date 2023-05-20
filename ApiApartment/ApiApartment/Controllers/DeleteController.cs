using ApiApartment.Context;
using Microsoft.AspNetCore.Mvc;

namespace ApiApartment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DeleteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
