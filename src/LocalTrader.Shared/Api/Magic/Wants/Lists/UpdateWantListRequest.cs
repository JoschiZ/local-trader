using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Data.Account;

namespace LocalTrader.Shared.Api.Magic.Wants.Lists;

public sealed class UpdateWantListRequest
{
    [BindFrom("wantListId"), RouteParam, JsonIgnore]
    public MagicWantListId Id { get; init; }
    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; init; }

    public string? Name { get; set; }
    public Accessibility? Accessibility { get; set; }
}

public sealed class UpdateWantListRequestValidator : AbstractValidator<UpdateWantListRequest>
{
    public UpdateWantListRequestValidator()
    {
        RuleFor(x => x.Name).MinimumLength(5).MaximumLength(50);
    }
}