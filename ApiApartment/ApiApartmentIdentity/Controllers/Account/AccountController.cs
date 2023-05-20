using ApiApartmentIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)           //совет от gpt
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;        //совет от gpt
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountInfo()
        {
            // Получение информации о текущем пользователе из идентификационного контекста
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            return Ok(user);
        }

        [HttpPut("FirstName")]
        public async Task<IActionResult> UpdateFirstName([FromBody] User updatedUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            user.FirstName = updatedUser.FirstName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPut("LastName")]
        public async Task<IActionResult> UpdateLastName([FromBody] User updatedUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            user.LastName = updatedUser.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPut("City")]
        public async Task<IActionResult> UpdateCity([FromBody] User updatedUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            user.City = updatedUser.City;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
