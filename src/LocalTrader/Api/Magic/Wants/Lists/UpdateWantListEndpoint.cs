using FastEndpoints;
using LocalTrader.Api.Account;
using LocalTrader.Data;
using LocalTrader.Shared.Api;

using LocalTrader.Shared.Api.Magic.Wants.Lists;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Wants.Lists;

internal sealed class UpdateWantListEndpoint : Endpoint<UpdateWantListRequest, Results<Ok, NotFound, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public UpdateWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(ApiRoutes.Magic.Wants.Lists.Update, ApiRoutes.Magic.Wants.Lists.UpdateBinding);
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(UpdateWantListRequest req, CancellationToken ct)
    {
        var existingList = await _context
            .Magic
            .WantLists
            .Where(x => x.UserId == req.UserId)
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