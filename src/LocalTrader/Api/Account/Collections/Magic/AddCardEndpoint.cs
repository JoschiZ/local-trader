using FastEndpoints;
using LocalTrader.Api.Cards.Magic;
using LocalTrader.Components.Account;
using LocalTrader.Data;
using LocalTrader.Shared.Api.Account.Collections.Magic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LocalTrader.Api.Account.Collections.Magic;


internal sealed class AddCardEndpoint : Endpoint<AddMagicCardToCollectionRequest, Results<Created, NotFound>>
{
    private readonly IMagicCardRepository _magicCardRepository;
    private readonly IdentityUserAccessor _userAccessor;
    private readonly TraderContext _context;

    public AddCardEndpoint(IMagicCardRepository magicCardRepository, IdentityUserAccessor userAccessor, TraderContext context)
    {
        _magicCardRepository = magicCardRepository;
        _userAccessor = userAccessor;
        _context = context;
    }

    public override void Configure()
    {
        Get("collection/add-card");
    }

    public override async Task<Results<Created, NotFound>> ExecuteAsync(AddMagicCardToCollectionRequest req, CancellationToken ct)
    {
        var user = await _userAccessor
            .GetRequiredUserAsync(HttpContext)
            .ConfigureAwait(false);
        
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
            UserId = user.Id,
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