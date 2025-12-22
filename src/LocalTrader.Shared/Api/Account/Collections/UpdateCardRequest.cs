namespace LocalTrader.Shared.Api.Account.Collections;

public class UpdateCardRequest
{
    public required CollectionCardId CardId { get; set; }
    public int? Quantity { get; set; }
}