using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using JetBrains.Annotations;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Constants;
using LocalTrader.Shared.Data.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserId = LocalTrader.Data.Account.UserId;
using WantedMagicCardId = LocalTrader.Data.Magic.WantedMagicCardId;

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

        var foundCards = await _context
            .Magic
            .Collections
            .FromSql($"""
                      SELECT * FROM "Magic"."CollectionCards"
                      JOIN "Magic"."WantedCards" WC ON WC."Id" = {req.WantedCardId}
                      JOIN public."AspNetUsers" U on U."Id" = "CollectionCards"."UserId"
                      WHERE WC."CardId" = {req.WantedCardId}
                      AND "Condition" >= WC."MinimumCondition"
                      AND ll_to_earth(U."Location_Latitude", U."Location_Langitude") -- location of the owner of the card
                              <@ earth_box(ll_to_earth({searchRadius.Latitude}, {searchRadius.Longitude}), {LocationConstants.MaximumRadius}) -- Is inside the allowed bounding box
                      AND earth_distance(ll_to_earth(U."Location_Latitude", U."Location_Langitude"), ll_to_earth({searchRadius.Latitude}, {searchRadius.Longitude}))  -- distance between the owner and searcher
                              <= {searchRadius.Radius} + U."Location_Radius"
                      ORDER BY earth_distance(ll_to_earth(U."Location_Latitude", U."Location_Langitude"), ll_to_earth({searchRadius.Latitude}, {searchRadius.Longitude}))-- is smaller than the allowed radius
                      """)
            .GroupBy(x => x.UserId)
            .Take(20)
            .ToArrayAsync(cancellationToken: ct)
            .ConfigureAwait(false);


        return TypedResults.Ok(new ResponseDto(foundCards));
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
    public sealed class ResponseDto(IGrouping<UserId, CollectionMagicCard>[] foundCards)
    {
        public IGrouping<UserId, CollectionMagicCard>[] FoundCards => foundCards;
    }
}