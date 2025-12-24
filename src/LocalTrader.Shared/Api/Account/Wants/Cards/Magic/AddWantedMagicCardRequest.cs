using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Api.Account.Collections;
using LocalTrader.Shared.Api.Account.Users;
using LocalTrader.Shared.Api.Account.Wants.WantLists;
using LocalTrader.Shared.Data.Cards.Magic;

namespace LocalTrader.Shared.Api.Account.Wants.Cards.Magic;

public sealed class AddWantedMagicCardRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; private init; }
    
    [BindFrom("wantListId"), JsonIgnore]
    public WantListId WantListId { get; private init; }
    
    public required ScryfallId ScryfallId { get; set; }
    public required int Quantity { get; set; }
    public required CardCondition MinimumCondition { get; set; }
}

public sealed class AddWantedCardRequestValidator : AbstractValidator<AddWantedMagicCardRequest>
{
    public AddWantedCardRequestValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(1);
    }
}