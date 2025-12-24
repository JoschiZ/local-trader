using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Shared.Data.Account;

namespace LocalTrader.Shared.Api.Magic.Wants.Lists;

public sealed class CreateWantListRequest
{
    public required string Name { get; set; }
    public Accessibility Accessibility { get; set; }

    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Accessibility
{
    Public,
    Private
}

public sealed class CreateWantListRequestValidator : AbstractValidator<CreateWantListRequest>
{
    public CreateWantListRequestValidator()
    {
        RuleFor(x => x.Name).MinimumLength(5).MaximumLength(50);
    }
}