namespace Nihr.Hub.Infrastructure.Interfaces;

public interface IGoogleAdminService
{
    Task<string> GetGoogleUserOuAsync(string userEmail);

    Task<bool> HasEnterpriseLicenseAsync(string userEmail);
}