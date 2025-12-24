using LocalTrader.Data.Magic;
using LocalTrader.Shared.Data.Magic.Cards;
using ScryfallApi.Client.Models;

namespace LocalTrader.Api.Magic;

public static class ScryfallCardExtensions
{
    extension(Card scryfallCard)
    {
        public MagicCard ToMagicCard()
            => new()
            {
                Name = scryfallCard.Name,
                ScryfallUrl = scryfallCard.ScryfallUri,
                ScryfallId = new ScryfallId(scryfallCard.Id),
            };
    }
}