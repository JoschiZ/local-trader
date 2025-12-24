using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Shared.Api;

using LocalTrader.Shared.Api.Magic.Wants.Cards;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Wants.Cards;

internal sealed class RemoveWantedCardEndpoint : Endpoint<RemoveWantedCardRequest, Results<NoContent, NotFound>>
{
    private readonly TraderContext _context;

    public RemoveWantedCardEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(ApiRoutes.Magic.Wants.Cards.Remove, ApiRoutes.Magic.Wants.Cards.RemoveBinding);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(RemoveWantedCardRequest req, CancellationToken ct)
    {
        var deletedCount = await _context
            .Magic
            .WantedCards
            .Where(x => x.WantList!.UserId == req.UserId)
            .Where(x => x.Id == req.WantedMagicCardId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}