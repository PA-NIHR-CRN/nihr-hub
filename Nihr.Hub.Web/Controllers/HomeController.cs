using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nihr.Hub.Infrastructure.Interfaces;
using Nihr.Hub.Web.Models;

namespace Nihr.Hub.Web.Controllers;

public class HomeController(ILogger<HomeController> logger, IUserRepository userRepository) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;

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
        var fullName = User?.FindFirst(ClaimTypes.Name)?.Value;
        var givenName = User?.FindFirst(ClaimTypes.GivenName)?.Value;
        var email = User?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace((email)))
        {
            throw new InvalidOperationException("User does not have an email address.");
        }
        
        var user = await this._userRepository.GetUser(email, cancellationToken);

        return View(new UserModel { FullName = fullName, GivenName = givenName, Email = email });
    }

    [Authorize]
    [Route("aup")]
    public async Task<IActionResult> AcceptAUP()
    {
        return this.Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}