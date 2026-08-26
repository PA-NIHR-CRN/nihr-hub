namespace Nihr.Hub.Web.Models;

public class PoliciesContent
{
    public IList<PolicyEntry> Policies { get; set; } = new List<PolicyEntry>();
}

public class PolicyEntry
{
    public required string Title { get; set; }
    public required string Url { get; set; }
    public required string Description { get; set; }
}
