using Microsoft.AspNetCore.Mvc;
using Test2Server.Data.Entities;
using Test2Server.Dtos;
using Test2Server.Services;
using Test2Server.Utils;

namespace Test2Server.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var hashedPassword = PasswordUtils.EncryptPassword(request.Password);

            var user = new UserEntity { Username = request.Username, Email = request.Email, Password = hashedPassword };

            var createUser = await _userService.CreateUser(user);

            return CreatedAtAction(nameof(Register), new { id = createUser.Uuid }, createUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Invalid input" });
            }

            var user = await _userService.GetOneUserByEmail(request.Email);

            if (user == null || user.DeletedAt != null || !PasswordUtils.ComparePassword(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = TokenUtils.GenerateToken(user.Uuid.ToString());

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            return Ok(new { Message = "Login successful" });
        }
    }
}