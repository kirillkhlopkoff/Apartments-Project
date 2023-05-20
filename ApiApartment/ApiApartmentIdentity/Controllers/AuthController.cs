using ApiApartmentIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiApartmentIdentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;     //совет от gpt

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)           //совет от gpt
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;        //совет от gpt
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration model)
        {
            var user = new User 
            { 
                UserName = model.UserName, 
                Email = model.Email, 
                FirstName = model.FirstName, 
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                City = model.City
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok();
        }

        [HttpPost("login")]              //это новый метод логин
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid login attempt.");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expires
            });
        }

        /*[HttpPost("login")]              //это старый метод логин
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid login attempt.");
            }

            return Ok();
        }*/

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
