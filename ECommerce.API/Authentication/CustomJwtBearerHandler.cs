using ECommerce.Application.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace ECommerce.API.Authentication
{
    public class CustomJwtBearerHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly JwtTokenService _jwtTokenService;

        public CustomJwtBearerHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, JwtTokenService jwtTokenService)
            : base(options, logger, encoder, clock)
        {
            _jwtTokenService = jwtTokenService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("No token provided"));
            }

            try
            {
                var principal = _jwtTokenService.ValidateToken(token);

                if (principal == null)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
                }

                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail("Token validation failed: " + ex.Message));
            }
        }
    }
}
