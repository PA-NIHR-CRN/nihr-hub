using System.Net;
using Google;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Licensing.v1;
using Microsoft.Extensions.Options;
using Nihr.Hub.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Settings;

namespace Nihr.Hub.Infrastructure.Services;

public class GoogleAdminService(
    DirectoryService directoryService,
    LicensingService licensingService,
    IOptions<GoogleAdminSettings> googleAdminSettings
) : IGoogleAdminService
{
    private readonly GoogleAdminSettings _googleAdminSettings = googleAdminSettings.Value;
    
    public async Task<string> GetGoogleUserOuAsync(string userEmail)
    {
        var user = await directoryService.Users.Get(userEmail).ExecuteAsync();
        return user.OrgUnitPath;
    }

    public async Task<bool> HasEnterpriseLicenseAsync(string userEmail)
    {
        foreach (var skuId in _googleAdminSettings.EnterpriseSkuIds)
        {
            if (await HasSkuAsync(skuId, userEmail))
            {
                return true;
            }
        }

        return false;
    }

    private async Task<bool> HasSkuAsync(string skuId, string userEmail)
    {
        try
        {
            await licensingService.LicenseAssignments.Get(
                productId: _googleAdminSettings.ProductId,
                skuId: skuId,
                userId: userEmail
            ).ExecuteAsync();

            return true;
        }
        catch (GoogleApiException ex)
            when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}