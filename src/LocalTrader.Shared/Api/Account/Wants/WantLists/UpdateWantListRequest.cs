using FastEndpoints;
using FluentValidation;

namespace LocalTrader.Shared.Api.Account.Wants.WantLists;

public sealed class UpdateWantListRequest
{
    [BindFrom("wantListId")]
    public WantListId Id { get; set; }
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