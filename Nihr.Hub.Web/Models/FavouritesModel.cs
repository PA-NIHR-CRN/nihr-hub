using System.Text.Json.Serialization;
using NIHR.Infrastructure.Authentication.IDG.SCIM.Models;

namespace Nihr.Hub.Web.Models;

public class FavouritesModel
{
    [JsonPropertyName("favouriteIds")] public List<int> FavouriteIds { get; set; } = [];
}