using System.ComponentModel.DataAnnotations;

namespace Nihr.Hub.Infrastructure.Settings;

public class PoliciesSettings
{
    [Required]
    public required IList<PolicyItemSettings> Items { get; set; }
}

public class PolicyItemSettings
{
    [Required]
    public required string PolicyName { get; set; }

    [Required]
    public required string PolicyDescription { get; set; }

    [Required]
    public required string PolicyUrl { get; set; }
}
