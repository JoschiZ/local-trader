using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Data;
using LocalTrader.Data.Magic;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MagicWantListId = LocalTrader.Data.Magic.MagicWantListId;
using ScryfallId = LocalTrader.Data.Magic.ScryfallId;
using UserId = LocalTrader.Data.Account.UserId;

namespace LocalTrader.Api.Magic.Wants.Cards;

internal sealed class AddWantedMagicCardEndpoint : Endpoint<AddWantedMagicCardEndpoint.Request, Results<Ok, UnauthorizedHttpResult, NotFound, Conflict>>
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
        Put(ApiRoutes.Magic.Wants.Cards.Add, x => new {x.ScryfallId});
    }

    public override async Task<Results<Ok, UnauthorizedHttpResult, NotFound, Conflict>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var alreadyExists = await _context
            .Magic
            .WantedCards
            .Where(x => x.Card!.ScryfallId == req.ScryfallId)
            .Where(x => x.WantListId == req.WantListId)
            .AnyAsync(ct)
            .ConfigureAwait(false);

        if (alreadyExists)
        {
            return TypedResults.Conflict();
        }
        
        var wantList = await _context
            .Magic
            .WantLists
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


        wantList.Cards.Add(newCard);

        await _context.SaveChangesAsync(ct).ConfigureAwait(false);

        return TypedResults.Ok();
    }
    
    public sealed class Request
    {
        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; init; }
    
        [BindFrom("wantListId"), RouteParam]
        public MagicWantListId WantListId { get; init; }
    
        public required ScryfallId ScryfallId { get; set; }
        public required int Quantity { get; set; }
        public required CardCondition MinimumCondition { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Quantity).GreaterThan(1);
        }
    }
}