using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Data.Account.Wants.Lists;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Account.Wants.WantLists;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LocalTrader.Api.Account.Wants.Lists;

internal sealed class CreateWantListEndpoint : Endpoint<CreateWantListRequest, Results<Created, Conflict, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public CreateWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Put(ApiRoutes.Account.WantLists.Create);
    }

    public override async Task<Results<Created, Conflict, UnauthorizedHttpResult>> ExecuteAsync(CreateWantListRequest req, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        var newWantList = new WantList
        {
            Name = req.Name,
            Accessibility = req.Accessibility,
            UserId = userId.Value
        };

        _context
            .Wants
            .Lists
            .Add(newWantList);

        await _context
            .SaveChangesAsync(ct)
            .ConfigureAwait(false);
        
        return TypedResults.Created();
    }
}