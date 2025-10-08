using System.ComponentModel.DataAnnotations;

namespace Nihr.Hub.Infrastructure.Settings;

public class GoogleAdminSettings
{
    [Required] public required string KeyJson { get; set; }
    [Required] public required string AdminToImpersonate { get; set; }
}