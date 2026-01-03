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
using MagicWantListId = LocalTrader.Data.Magic.MagicWantListId;
using UserId = LocalTrader.Data.Account.UserId;

namespace LocalTrader.Api.Magic.Wants.Lists;

internal sealed class DeleteWantListEndpoint : Endpoint<DeleteWantListEndpoint.Request, Results<NoContent, UnauthorizedHttpResult, NotFound>>
{
    private readonly TraderContext _context;

    public DeleteWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(ApiRoutes.Magic.Wants.Lists.Delete, x => new { x.Id });
    }
    
    public override async Task<Results<NoContent, UnauthorizedHttpResult, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var deletedCount = await _context
            .Magic
            .WantLists
            .Where(x => x.UserId == req.UserId)
            .Where(x => x.Id == req.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    
    public sealed class Request
    {
        [BindFrom("wantListId"), RouteParam]
        public required MagicWantListId Id { get; set; }

        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; set; }
    }

}