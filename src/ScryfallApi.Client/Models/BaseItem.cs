using System.Text.Json.Serialization;

namespace ScryfallApi.Client.Models;


public class BaseItem
{
    [JsonPropertyName("object")]
    public required string ObjectType { get; set; }
}
