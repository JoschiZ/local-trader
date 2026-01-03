using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using CollectionMagicCardId = LocalTrader.Data.Magic.CollectionMagicCardId;
using UserId = LocalTrader.Data.Account.UserId;

namespace LocalTrader.Api.Magic.Collection;

public class UpdateCardEndpoint : Endpoint<UpdateCardEndpoint.Request, Results<Ok, NotFound, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public UpdateCardEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(ApiRoutes.Magic.Collection.UpdateCard, x => new { x.CardId });
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(Request req, CancellationToken ct)
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
    
    public class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; init; }

        [BindFrom("cardId"), RouteParam]
        public required CollectionMagicCardId CardId { get; set; }
        public int? Quantity { get; set; }
    }
}