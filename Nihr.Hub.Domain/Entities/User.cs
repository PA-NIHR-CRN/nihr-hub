using Amazon.DynamoDBv2.DataModel;

namespace Nihr.Hub.Domain.Entities;

[DynamoDBTable("nihrd-dynamodb-nihr-hub-dev")]
public class User
{
    [DynamoDBHashKey] // Primary Key
    public string Email { get; set; } = string.Empty;

    [DynamoDBProperty] public string AupAcceptedVersion { get; set; } = string.Empty;

    [DynamoDBProperty] public DateTime AupAcceptedDate { get; set; }

    [DynamoDBProperty] public List<int> Favourites { get; set; } = new();
}