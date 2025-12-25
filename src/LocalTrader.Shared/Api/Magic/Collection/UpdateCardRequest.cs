using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using LocalTrader.Shared.Data.Account;
using LocalTrader.Shared.Data.Magic.Collection;

namespace LocalTrader.Shared.Api.Magic.Collection;

public class UpdateCardRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; private init; }

    [BindFrom("cardId"), RouteParam, JsonIgnore]
    public required CollectionMagicCardId CardId { get; set; }
    public int? Quantity { get; set; }
}