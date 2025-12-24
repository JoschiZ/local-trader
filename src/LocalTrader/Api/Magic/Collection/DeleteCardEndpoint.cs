using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using JetBrains.Annotations;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Data.Account;
using LocalTrader.Shared.Data.Magic.Collection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Collection;

internal sealed class DeleteCardEndpoint : Endpoint<DeleteCardEndpoint.Request, Results<NoContent, UnauthorizedHttpResult, NotFound>>
{
    private readonly TraderContext _context;

    public DeleteCardEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(ApiRoutes.Account.Collections.DeleteCard);
    }

    public override async Task<Results<NoContent, UnauthorizedHttpResult, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var delete = await _context
            .Magic
            .Collections
            .Where(c => c.UserId == req.UserId)
            .Where(x => x.Id == req.CardId)
            .ExecuteDeleteAsync(cancellationToken: ct)
            .ConfigureAwait(false);

        return delete > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    [PublicAPI]
    internal sealed class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
        public UserId UserId { get; private init; }
        [BindFrom("magicCardId"), JsonIgnore]
        public CollectionMagicCardId CardId { get; private init; }
    }
}
