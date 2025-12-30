using System.Security.Claims;
using System.Text.Json.Serialization;
using FastEndpoints;
using JetBrains.Annotations;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Magic.Wants.Cards;
using LocalTrader.Shared.Constants;
using LocalTrader.Shared.Data.Account;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Wants.Cards;

internal sealed class SearchForWantedCardEndpoint : Endpoint<SearchForWantedCardEndpoint.Request,
    Results<Ok<SearchForWantedCardEndpoint.ResponseDto>, NotFound>>
{
    private readonly TraderContext _context;

    public SearchForWantedCardEndpoint(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get(ApiRoutes.Magic.Wants.Cards.SearchForAvailableCards, x => new { x.WantedCardId });
    }

    public override async Task<Results<Ok<ResponseDto>, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var searchRadius = req.SearchRadius ?? await _context
            .Users
            .Where(x => x.Id == req.UserId)
            .Select(x => x.Location)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (searchRadius is null)
        {
            ThrowError(x => x.SearchRadius, "Either specify a search radius in the request or set a default one in your user account");
        }

        _context
            .Users
            .FromSql($"""
                     SELECT * FROM "dbo.AspNetUsers"
                     
                     """);


        return TypedResults.NotFound();
    }

    [PublicAPI(PublicApiMessages.Request)]
    public sealed class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier), JsonIgnore]
        public UserId UserId { get; set; }
        
        [RouteParam]
        public WantedMagicCardId WantedCardId { get; set; }
        
        /// <summary>
        /// A location to search from.
        /// This overwrites the location of the searching user
        /// </summary>
        public ActionRadius? SearchRadius { get; set; }
    }

    [PublicAPI(PublicApiMessages.Response)]
    public sealed class ResponseDto
    {
    }
}