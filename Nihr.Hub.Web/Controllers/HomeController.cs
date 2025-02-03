using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nihr.Hub.Domain.Entities;
using Nihr.Hub.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Settings;
using Nihr.Hub.Web.Extensions;
using Nihr.Hub.Web.Models;

namespace Nihr.Hub.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IUserRepository userRepository,
    IOptions<AupSettings> aupOptions,
    IOptions<HubApplicationSettings> hubApplicationOptions) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    private static readonly IList<int> DefaultApps = new List<int> { 1, 2, 3 };

    [Route("sign-in")]
    public IActionResult SignIn()
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = "/",
        }, "Google");
    }

    [Route("sign-out")]
    [Authorize]
    public new async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [Authorize]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var fullName = User?.GetFullName();
        var givenName = User?.GetGivenName();
        var email = User?.GetEmail();

        if (string.IsNullOrWhiteSpace((email)))
        {
            throw new InvalidOperationException("User does not have an email address.");
        }

        var aupCurrentVersion = aupOptions.Value.CurrentVersion;

        var user = await userRepository.GetUser(email, cancellationToken);

        if (user == null || user.AupAcceptedVersion != aupCurrentVersion)
        {
            return RedirectToAction("DisplayAup");
        }

        List<HubApplication> favouriteApps;

        if (user.Favourites.Count == 0)
        {
            favouriteApps = hubApplicationOptions.Value.Applications.Where(app => DefaultApps.Contains(app.Id))
                .ToList();
        }
        else
        {
            favouriteApps = hubApplicationOptions.Value.Applications.Where(app => user.Favourites.Contains(app.Id))
                .OrderBy(app => user.Favourites.IndexOf(app.Id))
                .ToList();
        }

        return View(new HomeModel
        {
            FullName = fullName, GivenName = givenName, Email = email,
            AllApplications = hubApplicationOptions.Value.Applications.Where(app => !favouriteApps.Contains(app))
                .ToList(),
            Favourites = favouriteApps
        });
    }

    [HttpPost]
    [Authorize]
    [Route("save-favourites")]
    public async Task<IActionResult> SaveFavourites([FromBody] FavouritesModel model,
        CancellationToken cancellationToken)
    {
        var email = User?.GetEmail();

        if (string.IsNullOrWhiteSpace((email)))
        {
            throw new InvalidOperationException("User does not have an email address.");
        }

        var user = await userRepository.GetUser(email, cancellationToken);

        user.Favourites = model.FavouriteIds;

        await userRepository.SaveUser(user, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [Route("aup")]
    [HttpGet]
    public IActionResult DisplayAup()
    {
        var aupCurrentVersion = aupOptions.Value.CurrentVersion;
        var aupUrl = aupOptions.Value.Url;
        return this.View(new AupModel { CurrentVersion = aupCurrentVersion, Url = aupUrl });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AcceptAup(CancellationToken cancellationToken)
    {
        var aupCurrentVersion = aupOptions.Value.CurrentVersion;
        var email = User?.GetEmail();

        if (string.IsNullOrWhiteSpace((email)))
        {
            throw new InvalidOperationException("User does not have an email address.");
        }

        var user = await userRepository.GetUser(email, cancellationToken);

        if (user != null)
        {
            user.AupAcceptedDate = DateTimeOffset.Now.ToString("o");
            user.AupAcceptedVersion = aupCurrentVersion;
        }
        else
        {
            user = new User
            {
                AupAcceptedDate = DateTimeOffset.Now.ToString("o"),
                AupAcceptedVersion = aupCurrentVersion,
                Email = email
            };
        }

        await userRepository.SaveUser(user, cancellationToken);

        return this.RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}