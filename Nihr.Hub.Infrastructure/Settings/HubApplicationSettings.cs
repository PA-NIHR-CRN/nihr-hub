namespace Nihr.Hub.Infrastructure.Settings;

public class HubApplicationSettings
{
    public required IList<HubApplication> Applications { get; set; }
}

public class HubApplication
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string IconImage { get; set; }
    public required string Url { get; set; }
}