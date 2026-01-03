using System;
using System.Linq;
using System.Linq.Expressions;
using LocalTrader.Api.Magic.Wants.Cards;
using LocalTrader.Data.Magic;

namespace LocalTrader.Api.Magic.Wants.Lists;

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