using ECommerce.API.Models;
using ECommerce.Application.Security;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(UserService userService, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {

            if (loginRequestModel == null)
            {
                return BadRequest("Invalid request.");
            }

            var user = await _userService.AuthenticateUserAsync(loginRequestModel.Username, loginRequestModel.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate JWT token
            var token = _jwtTokenService.GenerateToken(user.Username, user.Role.ToString(), user.Id);

            return Ok(new { Token = token });
        }
    }
}
