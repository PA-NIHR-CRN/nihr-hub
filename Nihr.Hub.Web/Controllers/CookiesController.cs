using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nihr.Hub.Web.Models;

namespace Nihr.Hub.Web.Controllers;

[Route("cookies")]
[Authorize]
public class CookiesController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var consent = Request.Cookies["CookieConsent"];

        var model = new CookiesPreferencesViewModel
        {
            CookieConsentGranted = consent
        };

        return View(model);
    }

    [HttpGet]
    [Route("policy")]
    public IActionResult Policy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(CookiesPreferencesViewModel model)
    {
        SetConsent(model.CookieConsentGranted == "true");
        TempData["ShowSuccessConfirmation"] = true;
        return RedirectToAction("Index");
    }

    [HttpPost("accept")]
    public IActionResult Accept()
    {
        SetConsent(true);
        TempData["ShowCookieConfirmation"] = "accepted";
        return Redirect(Request.Headers.Referer.ToString());
    }

    [HttpPost("reject")]
    public IActionResult Reject()
    {
        SetConsent(false);
        TempData["ShowCookieConfirmation"] = "rejected";
        return Redirect(Request.Headers.Referer.ToString());
    }

    private void SetConsent(bool accepted)
    {
        Response.Cookies.Append(
            "CookieConsent",
            accepted ? "true" : "false",
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            }
        );
    }
}