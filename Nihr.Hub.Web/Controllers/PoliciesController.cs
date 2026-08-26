using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NIHR.Infrastructure.Interfaces;
using Nihr.Hub.Web.Content;
using Nihr.Hub.Web.Models;

namespace Nihr.Hub.Web.Controllers;

[Authorize]
[Route("policies")]
public class PoliciesController(IContentProvider contentProvider) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var content = await contentProvider.GetContentAsync<PoliciesContent>(ContentIds.Policies, cancellationToken);
        return View(content);
    }
}
