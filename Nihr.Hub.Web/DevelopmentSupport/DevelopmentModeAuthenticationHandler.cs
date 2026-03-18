using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Nihr.Hub.Web
{
    public class DevelopmentModeAuthenticationHandler : AuthenticationHandler<DevelopmentModeAuthenticationOptions>
    {

        public DevelopmentModeAuthenticationHandler(
            IOptionsMonitor<DevelopmentModeAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder
        ) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Email, Options.Email),
                new Claim(ClaimTypes.GivenName, Options.GivenName),
                new Claim(ClaimTypes.Name, Options.Name),
            };

            var identity = new ClaimsIdentity(claims, nameof(DevelopmentModeAuthenticationHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}