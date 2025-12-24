using LocalTrader.Shared.Data.Magic.Collection;

namespace LocalTrader.Shared.Api.Magic.Collection;

public class UpdateCardRequest
{
    public required CollectionMagicCardId CardId { get; set; }
    public int? Quantity { get; set; }
}