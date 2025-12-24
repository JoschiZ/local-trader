using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Shared.Api;

using LocalTrader.Shared.Api.Magic.Wants.Cards;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Wants.Cards;

internal sealed class UpdateWantedCard : Endpoint<UpdateWantedCardRequest, Results<Ok, NotFound, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public UpdateWantedCard(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(ApiRoutes.Magic.Wants.Cards.Update, ApiRoutes.Magic.Wants.Cards.UpdateBinding);
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(UpdateWantedCardRequest req, CancellationToken ct)
    {
        var card = await _context
            .Magic
            .WantedCards
            .Where(x => x.WantList!.UserId == req.UserId)
            .FirstOrDefaultAsync(x => x.Id == req.WantedMagicCardId, ct)
            .ConfigureAwait(false);

        if (card is null)
        {
            return TypedResults.NotFound();
        }

        card.MinimumCondition = req.MinimumCondition ?? card.MinimumCondition;
        card.Quantity = req.Quantity ?? card.Quantity;

        await _context
            .SaveChangesAsync(ct)
            .ConfigureAwait(false);

        return TypedResults.Ok();
    }
}