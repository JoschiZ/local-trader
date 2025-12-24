using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Account.Wants.Cards;
using LocalTrader.Shared.Api.Magic.Wants.Cards;
using Microsoft.AspNetCore.Http.HttpResults;

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
        Delete(ApiRoutes.Account.WantLists.Cards.Remove);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(RemoveWantedCardRequest req, CancellationToken ct)
    {
        var deletedCount = await _context
            .Wants
            .WantedCards
            .Where(x => x.WantList!.UserId == req.UserId)
            .Where(x => x.Id == req.WantedMagicCardId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}