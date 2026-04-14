public interface IGoogleAdminService
{
    Task<string> GetGoogleUserOuAsync(string userEmail);

    Task<bool> HasEnterpriseLicenseAsync(string userEmail);
}