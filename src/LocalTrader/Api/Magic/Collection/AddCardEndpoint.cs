using FastEndpoints;
using LocalTrader.Api.Account;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;
using LocalTrader.Shared.Api.Magic.Collection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Magic.Collection;

internal sealed class AddCardEndpoint : Endpoint<AddMagicCardToCollectionRequest,
    Results<Created, NotFound, UnauthorizedHttpResult, Conflict>>
{
    private readonly TraderContext _context;
    private readonly IMagicCardRepository _magicCardRepository;

    public AddCardEndpoint(IMagicCardRepository magicCardRepository, TraderContext context)
    {
        _magicCardRepository = magicCardRepository;
        _context = context;
    }

    public override void Configure()
    {
        Put(ApiRoutes.Magic.Collection.AddCard);
    }

    public override async Task<Results<Created, NotFound, UnauthorizedHttpResult, Conflict>> ExecuteAsync(
        AddMagicCardToCollectionRequest req, CancellationToken ct)
    {
        var existingCard = await _context
            .Magic
            .Collections
            .Where(x => x.UserId == req.UserId)
            .Where(x => x.Condition == req.CardCondition)
            .Where(x => x.Card!.ScryfallId == req.ScryfallId)
            .AnyAsync(ct)
            .ConfigureAwait(false);

        if (existingCard) return TypedResults.Conflict();

        var cardResult = await _magicCardRepository
            .GetCardAsync(req.ScryfallId, ct)
            .ConfigureAwait(false);

        if (cardResult.IsFailure) return TypedResults.NotFound();
        var card = cardResult.Value;

        var collectionCard = new CollectionMagicCard
        {
            CardId = card.Id,
            Condition = req.CardCondition,
            Quantity = req.Quantity,
            UserId = req.UserId
        };

        _context
            .Magic
            .Collections
            .Add(collectionCard);

        await _context
            .SaveChangesAsync(ct)
            .ConfigureAwait(false);

        return TypedResults.Created();
    }
}