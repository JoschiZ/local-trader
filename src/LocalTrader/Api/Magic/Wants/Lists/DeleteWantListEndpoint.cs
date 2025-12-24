using FastEndpoints;
using LocalTrader.Api.Account;
using LocalTrader.Data;
using LocalTrader.Shared.Api;

using LocalTrader.Shared.Api.Magic.Wants.Lists;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Wants.Lists;

internal sealed class DeleteWantListEndpoint : Endpoint<DeleteWantListRequest, Results<NoContent, UnauthorizedHttpResult, NotFound>>
{
    private readonly TraderContext _context;

    public DeleteWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(ApiRoutes.Magic.Wants.Lists.Delete, ApiRoutes.Magic.Wants.Lists.DeleteBinding);
    }
    
    public override async Task<Results<NoContent, UnauthorizedHttpResult, NotFound>> ExecuteAsync(DeleteWantListRequest req, CancellationToken ct)
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
}