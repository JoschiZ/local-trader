using FastEndpoints;
using LocalTrader.Api.Cards.Magic;
using LocalTrader.Data;
using LocalTrader.Shared.Api.Account.Collections.Magic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Api.Account.Collections.Magic;


internal sealed class AddCardEndpoint : Endpoint<AddMagicCardToCollectionRequest, Results<Created, NotFound, UnauthorizedHttpResult, Conflict>>
{
    private readonly IMagicCardRepository _magicCardRepository;
    private readonly TraderContext _context;

    public AddCardEndpoint(IMagicCardRepository magicCardRepository, TraderContext context)
    {
        _magicCardRepository = magicCardRepository;
        _context = context;
    }

    public override void Configure()
    {
        Put("collection/add-card");
    }

    public override async Task<Results<Created, NotFound, UnauthorizedHttpResult, Conflict>> ExecuteAsync(AddMagicCardToCollectionRequest req, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        if (userId is null)
        {
            return TypedResults.Unauthorized();
        }

        var existingCard = await _context
            .Collections
            .Magic
            .Where(x => x.UserId == userId)
            .Where(x => x.Condition == req.CardCondition)
            .Where(x => x.Card!.ScryfallId == req.ScryfallId)
            .AnyAsync(ct)
            .ConfigureAwait(false);
        
        if (existingCard)
        {
            return TypedResults.Conflict();
        }
        
        var cardResult = await _magicCardRepository
            .GetCardAsync(req.ScryfallId, ct)
            .ConfigureAwait(false);

        if (cardResult.IsFailure)
        {
            return TypedResults.NotFound();
        }
        var card = cardResult.Value;

        var collectionCard = new CollectionMagicCard
        {
            CardId = card.Id,
            Name = card.Name,
            Condition = req.CardCondition,
            Quantity = req.Quantity,
            UserId = userId.Value,
        };
        
        _context
            .Collections
            .Magic
            .Add(collectionCard);

        await _context
            .SaveChangesAsync(ct)
            .ConfigureAwait(false);
        
        return TypedResults.Created();
    }
}