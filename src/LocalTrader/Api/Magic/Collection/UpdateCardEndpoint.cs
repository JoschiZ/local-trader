using FastEndpoints;
using LocalTrader.Api.Account;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Magic.Collection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Collection;

public class UpdateCardEndpoint : Endpoint<UpdateCardRequest, Results<Ok, NotFound, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public UpdateCardEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(ApiRoutes.Magic.Collection.UpdateCard, ApiRoutes.Magic.Collection.UpdateBinding);
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(UpdateCardRequest req, CancellationToken ct)
    {
        var card = await _context
            .Magic
            .Collections
            .Where(x => x.UserId == req.UserId)
            .FirstOrDefaultAsync(x => x.Id == req.CardId, cancellationToken: ct)
            .ConfigureAwait(false);

        if (card is null)
        {
            return TypedResults.NotFound();
        }

        card.Quantity = req.Quantity ?? card.Quantity;

        await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok();
    }
}