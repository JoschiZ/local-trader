using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using LocalTrader.Shared.Data.Account;

namespace LocalTrader.Shared.Api.Magic.Wants.Cards;

public sealed class RemoveWantedCardRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; init; }
    [BindFrom("wantedCardId"),RouteParam, JsonIgnore]
    public required WantedMagicCardId WantedMagicCardId { get; set; }
}