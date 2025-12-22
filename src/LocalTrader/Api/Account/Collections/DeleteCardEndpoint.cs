using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Account.Collections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Account.Collections;

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
        var userId = HttpContext.GetUserId();
        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        var delete = await _context
            .Collections
            .All
            .Where(c => c.UserId == userId)
            .Where(x => x.Id == req.CardId)
            .ExecuteDeleteAsync(cancellationToken: ct)
            .ConfigureAwait(false);

        return delete > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }


    internal sealed record Request(CollectionCardId CardId);
}
