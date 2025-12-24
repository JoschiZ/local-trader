using FastEndpoints;

namespace LocalTrader.Shared.Api.Magic.Wants.Lists;

public sealed class DeleteWantListRequest
{
    [BindFrom("wantListId")]
    public required MagicWantListId Id { get; set; }
}
