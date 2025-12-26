using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Api.Magic.Wants.Lists;
using LocalTrader.Shared.Data.Account;
using LocalTrader.Shared.Data.Magic.Cards;
using LocalTrader.Shared.Data.Shared;

namespace LocalTrader.Shared.Api.Magic.Wants.Cards;

public sealed class AddWantedMagicCardRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; init; }
    
    [BindFrom("wantListId"), RouteParam, JsonIgnore]
    public MagicWantListId WantListId { get; init; }
    
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