using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using LocalTrader.Shared.Data.Account;

namespace LocalTrader.Shared.Api.Magic.Wants.Lists;

public sealed class DeleteWantListRequest
{
    [BindFrom("wantListId"), JsonIgnore]
    public required MagicWantListId Id { get; set; }

    [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
    public UserId UserId { get; set; }
}
