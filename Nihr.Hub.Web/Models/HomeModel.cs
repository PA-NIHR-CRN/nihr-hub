using Nihr.Hub.Infrastructure.Settings;

namespace Nihr.Hub.Web.Models;

public class HomeModel
{
    public string? FullName { get; init; }

    public string? GivenName { get; init; }
    public string? Email { get; init; }

    public IList<HubApplication> Favourites { get; init; } = new List<HubApplication>();
    public IList<HubApplication> AllApplications { get; init; } = new List<HubApplication>();
}