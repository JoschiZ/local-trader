using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Account.Wants.WantLists;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Account.Wants.Lists;

internal sealed class UpdateWantListEndpoint : Endpoint<UpdateWantListRequest, Results<Ok, NotFound, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public UpdateWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(ApiRoutes.Account.WantLists.Update);
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(UpdateWantListRequest req, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        var existingList = await _context
            .Wants
            .Lists
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: ct)
            .ConfigureAwait(false);

        if (existingList is null)
        {
            return TypedResults.NotFound();
        }

        existingList.Name = req.Name ?? existingList.Name;
        existingList.Accessibility = req.Accessibility ?? existingList.Accessibility;

        await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok();
    }
}