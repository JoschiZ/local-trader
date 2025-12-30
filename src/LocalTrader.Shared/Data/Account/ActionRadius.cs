using System.ComponentModel.DataAnnotations.Schema;

namespace LocalTrader.Shared.Data.Account;

[ComplexType]
public record ActionRadius
{
    public required Kilometers Radius { get; set; }
    public required decimal Langitude { get; init; }
    public required decimal Latitude { get; init; }
}
