using Microsoft.AspNetCore.Authentication;

namespace Nihr.Hub.Web
{
    public class DevelopmentModeAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Email { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}