using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;

namespace Nihr.Hub.Domain.Entities;

public class User
{
    [DynamoDBHashKey] public string Email { get; set; } = string.Empty; // PK in Dynamo

    [DynamoDBProperty] public string AupAcceptedVersion { get; set; } = string.Empty;

    [DynamoDBProperty] public string AupAcceptedDate { get; set; } = string.Empty;

    [DynamoDBProperty]
    public string FavouritesJson
    {
        get => JsonSerializer.Serialize(Favourites);
        set => Favourites = JsonSerializer.Deserialize<List<int>>(value) ?? [];
    }

    [DynamoDBIgnore] public List<int> Favourites { get; set; } = [];

    [DynamoDBProperty] public string LastUpdatedDate { get; set; } = string.Empty;
}