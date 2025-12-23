using FastEndpoints;
using FluentValidation;

namespace LocalTrader.Shared.Api.Account.Wants.WantLists;

public sealed class DeleteWantListRequest
{
    [BindFrom("wantListId")]
    public required WantListId Id { get; set; }
}
