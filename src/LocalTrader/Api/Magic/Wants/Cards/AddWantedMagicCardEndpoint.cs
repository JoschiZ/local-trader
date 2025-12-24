using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Magic.Wants.Cards;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LocalTrader.Api.Magic.Wants.Cards;

internal sealed class AddWantedMagicCardEndpoint : Endpoint<AddWantedMagicCardRequest, Results<Ok, UnauthorizedHttpResult, NotFound, Conflict>>
{
    private readonly TraderContext _context;
    private readonly IMagicCardRepository _cardRepository;

    public AddWantedMagicCardEndpoint(TraderContext context, IMagicCardRepository cardRepository)
    {
        _context = context;
        _cardRepository = cardRepository;
    }

    public override void Configure()
    {
        Put(ApiRoutes.Account.WantLists.Cards.Magic.Add);
    }

    public override async Task<Results<Ok, UnauthorizedHttpResult, NotFound, Conflict>> ExecuteAsync(AddWantedMagicCardRequest req, CancellationToken ct)
    {
        var alreadyExists = await _context
            .Wants
            .WantedMagicCards
            .Where(x => x.Card!.ScryfallId == req.ScryfallId)
            .Where(x => x.WantListId == req.WantListId)
            .AnyAsync(ct)
            .ConfigureAwait(false);

        if (alreadyExists)
        {
            return TypedResults.Conflict();
        }
        
        var wantList = await _context
            .Wants
            .Lists
            .Where(x => x.UserId == req.UserId)
            .FirstOrDefaultAsync(x => x.Id == req.WantListId, cancellationToken: ct)
            .ConfigureAwait(false);

        if (wantList is null)
        {
            return TypedResults.NotFound();
        }

        var card = await _cardRepository
            .GetCardAsync(req.ScryfallId, ct)
            .ConfigureAwait(false);

        if (card.IsFailure)
        {
            return TypedResults.NotFound();
        }

        var newCard = new WantedMagicCard
        {
            CardId = card.Value.Id,
            WantListId = req.WantListId,
            MinimumCondition = req.MinimumCondition,
            Quantity = req.Quantity
        };
        
        wantList.WantedCards.Add(newCard);

        await _context.SaveChangesAsync(ct).ConfigureAwait(false);

        return TypedResults.Ok();
    }
}