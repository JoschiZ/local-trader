using LocalTrader.Data.Magic;

using ScryfallApi.Client.Models;
using ScryfallId = LocalTrader.Data.Magic.ScryfallId;

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