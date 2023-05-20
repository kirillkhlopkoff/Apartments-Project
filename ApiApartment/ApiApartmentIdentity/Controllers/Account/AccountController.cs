using ApiApartmentIdentity.Context;
using ApiApartmentIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiApartmentIdentity.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Требуется аутентификация для доступа к методам этого контроллера
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;     //совет от gpt
        private readonly UsersDbContext _context;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            UsersDbContext context)           //совет от gpt
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;        //совет от gpt
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountInfo()
        {
            // Получение информации о текущем пользователе из идентификационного контекста
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            return Ok(user);
        }

        [HttpPut("api/Account/UpdateUser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.City = user.City;
                    // Обновите другие поля пользователя по аналогии

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return NotFound(); // Обработайте ситуацию, если пользователя не найдено
                }

                // В случае использования другого хранилища данных или сервиса, обновите код в соответствии с вашей реализацией.
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Обработайте ошибку сервера
            }
        }
    }
}
