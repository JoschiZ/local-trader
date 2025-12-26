using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using LocalTrader.Shared.Data.Account;
using LocalTrader.Shared.Data.Magic.Cards;
using LocalTrader.Shared.Data.Shared;

namespace LocalTrader.Shared.Api.Magic.Collection;

public sealed class AddMagicCardToCollectionRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; init; }
    public required ScryfallId ScryfallId { get; set; }
    public int Quantity { get; set; } = 1;
    public required CardCondition CardCondition { get; set; }
}

