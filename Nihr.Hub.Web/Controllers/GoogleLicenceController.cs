using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nihr.Hub.Infrastructure.Interfaces;

namespace Nihr.Hub.Web.Controllers;

[ApiController]
[Route("api/google/licence")]
[Authorize] 
public class GoogleLicenceController : ControllerBase
{
    private readonly IGoogleAdminService _googleAdminService;

    public GoogleLicenceController(IGoogleAdminService googleAdminService)
    {
        _googleAdminService = googleAdminService;
    }

    [HttpGet("workspace-enterprise")]
    public async Task<IActionResult> HasWorkspaceEnterpriseLicence()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var hasLicence =
            await _googleAdminService.HasEnterpriseLicenseAsync(email);

        if (hasLicence)
            return Ok();        

        return NotFound();     
    }
}