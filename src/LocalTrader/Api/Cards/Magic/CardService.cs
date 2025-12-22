using System.Threading;
using System.Threading.Tasks;
using LocalTrader.Data;
using LocalTrader.Data.Cards.Magic;
using LocalTrader.Shared.Data.Cards.Magic;
using LocalTrader.Shared.DataStructures.Results;
using Microsoft.EntityFrameworkCore;
using ScryfallApi.Client;
using ScryfallApi.Client.Models;
using Error = LocalTrader.Shared.DataStructures.Results.Error;

namespace LocalTrader.Api.Cards.Magic;

public interface IMagicCardRepository
{
    Task<Result<MagicCard>> GetCardAsync(ScryfallId scryfallId, CancellationToken cancellationToken = default);
}

internal sealed class MagicCardRepository : IMagicCardRepository
{
    private readonly TraderContext _context;
    private readonly ScryfallApiClient _scryfallClient;

    public MagicCardRepository(TraderContext context, ScryfallApiClient scryfallClient)
    {
        _context = context;
        _scryfallClient = scryfallClient;
    }

    public async Task<Result<MagicCard>> GetCardAsync(ScryfallId scryfallId, CancellationToken cancellationToken = default)
    {
        var card = await _context
            .Cards
            .Magic
            .FirstOrDefaultAsync(x => x.ScryfallId == scryfallId,  cancellationToken)
            .ConfigureAwait(false);

        if (card is not null)
        {
            return card;
        }

        var scryfallCard = await _scryfallClient
            .Cards
            .ById(scryfallId.Value)
            .ConfigureAwait(false);

        if (scryfallCard.IsError)
        {
            return Error.NotFound;
        }

        card = scryfallCard.Value.ToMagicCard();

        _context
            .Cards
            .Magic
            .Add(card);
        
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return card;
    }
}

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