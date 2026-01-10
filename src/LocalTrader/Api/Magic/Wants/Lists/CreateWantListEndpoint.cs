using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using UserId = LocalTrader.Data.Account.UserId;

namespace LocalTrader.Api.Magic.Wants.Lists;

internal sealed class CreateWantListEndpoint : Endpoint<CreateWantListEndpoint.Request, Created>
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

    public override async Task<Created> ExecuteAsync(Request req, CancellationToken ct)
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
    
    public sealed class Request
    {
        public required string Name { get; set; }
        public WantListAccessibility Accessibility { get; set; }
        /// <summary>
        /// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier: 019b835b-628c-74f1-ab65-cf8a059748bf
        /// </summary>
        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; set; }
    }
    
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).MinimumLength(5).MaximumLength(50);
        }
    }
}