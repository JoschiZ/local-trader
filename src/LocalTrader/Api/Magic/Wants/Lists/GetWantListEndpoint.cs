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
internal sealed class GetWantListEndpoint : Endpoint<GetWantListEndpoint.Request, Results<Ok<WantListDto>, NotFound>>
{
    private readonly TraderContext _context;

    public GetWantListEndpoint(TraderContext context)
    {
        _context = context;
    }
    public override void Configure()
    {
        Get(ApiRoutes.Magic.Wants.Lists.Get, x => new {x.WantListId});   
        AllowAnonymous();
    }

    public override async Task<Results<Ok<WantListDto>, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var list =  await _context
            .Magic
            .WantLists
            .Where(x => x.Accessibility == WantListAccessibility.Public || x.UserId == req.UserId)
            .Where(x => x.Id == req.WantListId)
            .ProjectToDto()
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        return list is null ? TypedResults.NotFound() : TypedResults.Ok(list);
    }
    
    public sealed class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier, false)]
        public UserId UserId { get; init; }
    
        [BindFrom("wantListId"), RouteParam]
        public MagicWantListId WantListId { get; init; }
    }

    public sealed class GetWantListRequestValidator : AbstractValidator<Request>
    {
        public GetWantListRequestValidator()
        {
            RuleFor(x => x.WantListId).NotEmpty();
        }
    }
}