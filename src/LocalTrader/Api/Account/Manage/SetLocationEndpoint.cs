using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using JetBrains.Annotations;
using LocalTrader.Data;
using LocalTrader.Data.Account;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Constants;
using LocalTrader.Shared.Data.Account;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Account.Manage;

internal sealed class SetLocationEndpoint : Endpoint<SetLocationEndpoint.Request, Results<Ok, NotFound>>
{
    private readonly TraderContext _context;

    public SetLocationEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Put(ApiRoutes.Account.SetLocation);
    }

    public override async Task<Results<Ok, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var user = await _context
            .Users
            .Where(x => x.Id == req.UserId)
            .FirstOrDefaultAsync(ct).ConfigureAwait(false);

        if (user is null)
        {
            return TypedResults.NotFound();
        }
        
        user.Location = user.Location == req.Location ? user.Location : req.Location;
        user.ActionRadius = req.ActionRadius;
        await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok();
    }

    [PublicAPI(PublicApiMessages.Request)]
    public sealed class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
        public UserId UserId { get; set; } 
        
        public required ActionRadius Location { get; set; }
        public required Kilometers ActionRadius { get; set; }
    }
    
}