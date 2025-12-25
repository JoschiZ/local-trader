using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Data.Account;
using LocalTrader.Shared.Data.Shared;

namespace LocalTrader.Shared.Api.Magic.Wants.Cards;

public sealed class UpdateWantedCardRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; init; }
    
    [BindFrom("wantedCardId"), RouteParam, JsonIgnore]
    public required WantedMagicCardId WantedMagicCardId { get; set; }
    
    public int? Quantity { get; set; }
    public CardCondition? MinimumCondition { get; set; }
}

public sealed class UpdateWantedCardRequestValidator : AbstractValidator<UpdateWantedCardRequest>
{
    public UpdateWantedCardRequestValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Use the delete endpoint instead");
    }
}