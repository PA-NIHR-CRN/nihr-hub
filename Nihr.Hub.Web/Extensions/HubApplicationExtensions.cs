using Nihr.Hub.Infrastructure.Settings;

namespace Nihr.Hub.Web.Extensions;

public static class HubApplicationExtensions
{
    public static bool CanUserSee(this HubApplication app, string ou, bool userHasEnterpriseLicense)
    {
        return (app.AllowedOperatingUnits == null || app.AllowedOperatingUnits.Contains(ou))
               && (!app.RequiresEnterpriseLicense || userHasEnterpriseLicense);
    }
}