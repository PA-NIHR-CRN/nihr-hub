using System.Net;
using Google;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Licensing.v1;
using Nihr.Hub.Infrastructure.Interfaces;

namespace Nihr.Hub.Infrastructure.Services;

public class GoogleAdminService(
    DirectoryService directoryService,
    LicensingService licensingService
) : IGoogleAdminService
{
    private const string ProductId = "Google-Apps";
    private const string EnterpriseStandardSkuId = "1010020026";
    private const string EnterprisePlusSkuId = "1010020027";

    public async Task<string> GetGoogleUserOuAsync(string userEmail)
    {
        var user = await directoryService.Users.Get(userEmail).ExecuteAsync();
        return user.OrgUnitPath;
    }

    public async Task<bool> HasEnterpriseLicenseAsync(string userEmail)
    {
        return await HasSkuAsync(EnterpriseStandardSkuId, userEmail)
            || await HasSkuAsync(EnterprisePlusSkuId, userEmail);
    }

    private async Task<bool> HasSkuAsync(string skuId, string userEmail)
    {
        try
        {
            await licensingService.LicenseAssignments.Get(
                productId: ProductId,
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