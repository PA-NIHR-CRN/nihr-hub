using System.ComponentModel.DataAnnotations;

namespace Nihr.Hub.Infrastructure.Settings;

public class HubApplicationSettings
{
    [Required] public required IList<HubApplication> Applications { get; set; }
}

public class HubApplication
{
    [Required] public required int Id { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string ImageName { get; set; }
    [Required] public required string Url { get; set; }
}