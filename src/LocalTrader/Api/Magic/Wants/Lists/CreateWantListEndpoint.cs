using FastEndpoints;
using LocalTrader.Api.Account;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;

using LocalTrader.Shared.Api.Magic.Wants.Lists;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LocalTrader.Api.Magic.Wants.Lists;

internal sealed class CreateWantListEndpoint : Endpoint<CreateWantListRequest, Results<Created, Conflict, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public CreateWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Put(ApiRoutes.Magic.Wants.Lists.Create);
    }

    public override async Task<Results<Created, Conflict, UnauthorizedHttpResult>> ExecuteAsync(CreateWantListRequest req, CancellationToken ct)
    {
        var newWantList = new MagicWantList
        {
            Name = req.Name,
            Accessibility = req.Accessibility,
            UserId = req.UserId,
        };

        _context
            .Magic
            .WantLists
            .Add(newWantList);

        await _context
            .SaveChangesAsync(ct)
            .ConfigureAwait(false);
        
        return TypedResults.Created();
    }
}