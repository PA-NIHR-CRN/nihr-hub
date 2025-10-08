using Google.Apis.Admin.Directory.directory_v1;
using Nihr.Hub.Infrastructure.Interfaces;

namespace Nihr.Hub.Infrastructure.Services;

public class GoogleAdminService(DirectoryService directoryService) : IGoogleAdminService
{
    public async Task<string> GetGoogleUserOuAsync(string userEmail)
    {
        // No need to create credentials or services here. Just use it.
        var user = await directoryService.Users.Get(userEmail).ExecuteAsync();
        return user.OrgUnitPath;
    }
}