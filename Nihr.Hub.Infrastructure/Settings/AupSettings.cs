using System.ComponentModel.DataAnnotations;

namespace Nihr.Hub.Infrastructure.Settings;

public class AupSettings
{
    [Required] public required string CurrentVersion { get; set; }

    [Required] public required string Url { get; set; }
}