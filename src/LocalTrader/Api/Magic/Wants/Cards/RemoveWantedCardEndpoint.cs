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
using UserId = LocalTrader.Data.Account.UserId;
using WantedMagicCardId = LocalTrader.Data.Magic.WantedMagicCardId;

namespace LocalTrader.Api.Magic.Wants.Cards;

internal sealed class RemoveWantedCardEndpoint : Endpoint<RemoveWantedCardEndpoint.Request, Results<NoContent, NotFound>>
{
    private readonly TraderContext _context;

    public RemoveWantedCardEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(ApiRoutes.Magic.Wants.Cards.Remove,x => new {x.WantedMagicCardId});
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
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
    
    public sealed class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; set; }
        [BindFrom("wantedCardId"),RouteParam]
        public required WantedMagicCardId WantedMagicCardId { get; set; }
    }
}