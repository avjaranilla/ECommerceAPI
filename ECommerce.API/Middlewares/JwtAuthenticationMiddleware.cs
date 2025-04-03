using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ECommerce.API.Middlewares
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await AttachUserToContext(httpContext, token);
            }

            await _next(httpContext);
        }
        private async Task AttachUserToContext(HttpContext httpContext, string token)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = System.TimeSpan.Zero // Optional: Adjust for token expiration tolerance
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                httpContext.User = principal; // Attach the user claims to the request context
            }
            catch
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        }
    }
}
