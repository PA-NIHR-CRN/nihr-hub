using System.ComponentModel.DataAnnotations;

namespace Nihr.Hub.Infrastructure.Settings;

public class BannerSettings
{
    public bool Enabled { get; set; }
    public string Message { get; set; } = string.Empty;
}
