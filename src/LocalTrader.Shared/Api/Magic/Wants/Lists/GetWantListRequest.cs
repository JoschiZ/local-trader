using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Data.Account;

namespace LocalTrader.Shared.Api.Magic.Wants.Lists;

public sealed class GetWantListRequest
{
    [FromClaim(ClaimTypes.NameIdentifier, false)]
    public UserId UserId { get; init; }
    
    [BindFrom("wantListId"), RouteParam, JsonIgnore]
    public MagicWantListId WantListId { get; init; }
}

public sealed class GetWantListRequestValidator : AbstractValidator<GetWantListRequest>
{
    public GetWantListRequestValidator()
    {
        RuleFor(x => x.WantListId).NotEmpty();
    }
}