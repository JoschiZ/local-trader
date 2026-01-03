using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MagicWantListId = LocalTrader.Data.Magic.MagicWantListId;
using UserId = LocalTrader.Data.Account.UserId;

namespace LocalTrader.Api.Magic.Wants.Lists;

internal sealed class UpdateWantListEndpoint : Endpoint<UpdateWantListEndpoint.Request, Results<Ok, NotFound, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public UpdateWantListEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(ApiRoutes.Magic.Wants.Lists.Update, x => new {x.Id});
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(Request req, CancellationToken ct)
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
    
    public sealed class Request
    {
        [BindFrom("wantListId"), RouteParam]
        public MagicWantListId Id { get; init; }
        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; init; }

        public string? Name { get; set; }
        public WantListAccessibility? Accessibility { get; set; }
    }

    public sealed class UpdateWantListRequestValidator : AbstractValidator<Request>
    {
        public UpdateWantListRequestValidator()
        {
            RuleFor(x => x.Name).MinimumLength(5).MaximumLength(50);
        }
    }
}