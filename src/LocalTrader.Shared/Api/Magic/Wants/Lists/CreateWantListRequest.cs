using System.Text.Json.Serialization;
using FluentValidation;

namespace LocalTrader.Shared.Api.Magic.Wants.Lists;

public sealed class CreateWantListRequest
{
    public required string Name { get; set; }
    public Accessibility Accessibility { get; set; }
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