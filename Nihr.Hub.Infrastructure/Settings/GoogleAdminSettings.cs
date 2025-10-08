using System.ComponentModel.DataAnnotations;

namespace Nihr.Hub.Infrastructure.Settings;

public class GoogleAdminSettings
{
    [Required] public string KeyJson { get; set; }
    [Required] public string AdminToImpersonate { get; set; }
}