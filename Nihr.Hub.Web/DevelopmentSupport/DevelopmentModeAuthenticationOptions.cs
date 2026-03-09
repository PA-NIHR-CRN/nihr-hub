using Microsoft.AspNetCore.Authentication;

namespace Nihr.Hub.Web
{
    public class DevelopmentModeAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string Name { get; set; }
    }
}