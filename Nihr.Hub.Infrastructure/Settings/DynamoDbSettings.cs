using System.ComponentModel.DataAnnotations;

namespace Nihr.Hub.Infrastructure.Settings;

public class DynamoDbSettings
{
    [Required] public required string TableName { get; set; }
}