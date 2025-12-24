using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Api.Account.Users;

namespace LocalTrader.Shared.Api.Account.Wants.WantLists;

public sealed class GetWantListRequest
{
    [FromClaim(ClaimTypes.NameIdentifier, false)]
    public UserId? UserId { get; private init; }
    
    [BindFrom("wantListId"), JsonIgnore]
    public WantListId WantListId { get; private init; }
}

public sealed class GetWantListRequestValidator : AbstractValidator<GetWantListRequest>
{
    public GetWantListRequestValidator()
    {
        RuleFor(x => x.WantListId).NotEmpty();
    }
}