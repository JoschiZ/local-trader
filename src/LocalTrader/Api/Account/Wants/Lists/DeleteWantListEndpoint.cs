using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Account.Wants.WantLists;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Account.Wants.Lists;

internal sealed class DeleteWantListEndpoint : Endpoint<DeleteWantListRequest, Results<NoContent, UnauthorizedHttpResult, NotFound>>
{
    private readonly TraderContext _context;

    public DeleteWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(ApiRoutes.Account.WantLists.Delete);
    }
    
    public override async Task<Results<NoContent, UnauthorizedHttpResult, NotFound>> ExecuteAsync(DeleteWantListRequest req, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        var deletedCount = await _context
            .Wants
            .Lists
            .Where(x => x.UserId == userId.Value)
            .Where(x => x.Id == req.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}