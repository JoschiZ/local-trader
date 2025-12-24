using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Api.Account.Collections;
using LocalTrader.Shared.Api.Account.Users;

namespace LocalTrader.Shared.Api.Account.Wants.Cards;

public sealed class UpdateWantedCardRequest
{
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; init; }
    
    [BindFrom("wantedCardId"), JsonIgnore]
    public required WantedCardId WantedCardId { get; set; }
    
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