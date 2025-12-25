using System.Linq.Expressions;
using LocalTrader.Shared.Api.Magic.Wants.Lists;
using Microsoft.AspNetCore.Http.HttpResults;
using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Magic.Wants.Cards;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Wants.Lists;
internal sealed class GetWantListEndpoint : Endpoint<GetWantListRequest, Results<Ok<WantListDto>, NotFound>>
{
    private readonly TraderContext _context;

    public GetWantListEndpoint(TraderContext context)
    {
        _context = context;
    }
    public override void Configure()
    {
        Get(ApiRoutes.Magic.Wants.Lists.Get, ApiRoutes.Magic.Wants.Lists.GetBinding);   
        AllowAnonymous();
    }

    public override async Task<Results<Ok<WantListDto>, NotFound>> ExecuteAsync(GetWantListRequest req, CancellationToken ct)
    {
        var list =  await _context
            .Magic
            .WantLists
            .Where(x => x.Accessibility == Accessibility.Public || x.UserId == req.UserId)
            .Where(x => x.Id == req.WantListId)
            .ProjectToDto()
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        return list is null ? TypedResults.NotFound() : TypedResults.Ok(list);
    }
}

internal static class WantListMappings
{
    private static readonly Expression<Func<WantedMagicCard, WantedMagicCardDto>> CardProjection = card =>
        new WantedMagicCardDto(
            card.Card!.Name,
            card.Id,
            card.MinimumCondition,
            card.CardId,
            card.Card.ScryfallId,
            card.Card.ScryfallUrl);
        
    extension(IQueryable<MagicWantList> wantLists)
    {
        
        
        public IQueryable<WantListDto> ProjectToDto() => wantLists
            .Select(x => new WantListDto(
                x.Name,
                x.UserId,
                x.Accessibility,
                x.Cards.Select(CardProjection.Compile()).ToArray()
            ));
    }
}