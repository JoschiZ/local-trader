using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using LocalTrader.Shared.Api.Account.Users;

namespace LocalTrader.Shared.Api.Account.Wants.Cards;

public sealed class RemoveWantedCardRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; init; }
    [BindFrom("wantedCardId"), JsonIgnore]
    public required WantedCardId WantedCardId { get; set; }
}