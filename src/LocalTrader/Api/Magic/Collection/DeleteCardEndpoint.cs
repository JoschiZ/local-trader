using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using JetBrains.Annotations;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using CollectionMagicCardId = LocalTrader.Data.Magic.CollectionMagicCardId;
using UserId = LocalTrader.Data.Account.UserId;

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
        Delete(ApiRoutes.Magic.Collection.DeleteCard, x => new {x.CardId});
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
