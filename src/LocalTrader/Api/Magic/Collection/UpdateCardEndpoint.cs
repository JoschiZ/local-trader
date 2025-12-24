using FastEndpoints;
using LocalTrader.Api.Account;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Magic.Collection;
using Microsoft.AspNetCore.Http.HttpResults;

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
        Patch(ApiRoutes.Account.Collections.UpdateCard);
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(UpdateCardRequest req, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        var card = await _context
            .Collections
            .All
            .Where(x => x.UserId == userId)
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