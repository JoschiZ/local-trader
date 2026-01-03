using System.Text.Json.Serialization;

namespace LocalTrader.Api.Magic.Wants.Lists;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WantListAccessibility
{
    Public,
    Private
}