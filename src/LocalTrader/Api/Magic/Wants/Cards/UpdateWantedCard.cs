using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;
using LocalTrader.Data;
using LocalTrader.Shared.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserId = LocalTrader.Data.Account.UserId;
using WantedMagicCardId = LocalTrader.Data.Magic.WantedMagicCardId;

namespace LocalTrader.Api.Magic.Wants.Cards;

internal sealed class UpdateWantedCard : Endpoint<UpdateWantedCard.UpdateWantedCardRequest, Results<Ok, NotFound, UnauthorizedHttpResult>>
{
    private readonly TraderContext _context;

    public UpdateWantedCard(TraderContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(ApiRoutes.Magic.Wants.Cards.Update, x => new { x.WantedMagicCardId });
    }

    public override async Task<Results<Ok, NotFound, UnauthorizedHttpResult>> ExecuteAsync(UpdateWantedCardRequest req, CancellationToken ct)
    {
        var card = await _context
            .Magic
            .WantedCards
            .Where(x => x.WantList!.UserId == req.UserId)
            .FirstOrDefaultAsync(x => x.Id == req.WantedMagicCardId, ct)
            .ConfigureAwait(false);

        if (card is null)
        {
            return TypedResults.NotFound();
        }

        card.MinimumCondition = req.MinimumCondition ?? card.MinimumCondition;
        card.Quantity = req.Quantity ?? card.Quantity;

        await _context
            .SaveChangesAsync(ct)
            .ConfigureAwait(false);

        return TypedResults.Ok();
    }
    
    public sealed class UpdateWantedCardRequest
    {
        [FromClaim(ClaimTypes.NameIdentifier)]
        public UserId UserId { get; init; }
    
        [BindFrom("wantedCardId"), RouteParam]
        public required WantedMagicCardId WantedMagicCardId { get; set; }
    
        public int? Quantity { get; set; }
        public CardCondition? MinimumCondition { get; set; }
    }

    public sealed class UpdateWantedCardRequestValidator : AbstractValidator<UpdateWantedCardRequest>
    {
        public UpdateWantedCardRequestValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Use the delete endpoint instead");
        }
    }
}