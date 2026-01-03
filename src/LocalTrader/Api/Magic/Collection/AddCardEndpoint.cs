using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ScryfallId = LocalTrader.Data.Magic.ScryfallId;
using UserId = LocalTrader.Data.Account.UserId;

namespace LocalTrader.Api.Magic.Collection;

internal sealed class AddCardEndpoint : Endpoint<AddCardEndpoint.Request,
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
        Request req, CancellationToken ct)
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
    
    public sealed class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; init; }
        public required ScryfallId ScryfallId { get; set; }
        public int Quantity { get; set; } = 1;
        public required CardCondition CardCondition { get; set; }
    }
    
}