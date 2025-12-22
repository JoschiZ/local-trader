using LocalTrader.Shared.Data.Cards.Magic;

namespace LocalTrader.Shared.Api.Account.Collections.Magic;

public sealed class AddMagicCardToCollectionRequest
{
    public required ScryfallId ScryfallId { get; set; }
    public int Quantity { get; set; } = 1;
    public required CardCondition CardCondition { get; set; }
}

