using LocalTrader.Data.Account.Wants.Lists;
using LocalTrader.Shared.Api.Account.Collections;
using LocalTrader.Shared.Api.Account.Users;
using LocalTrader.Shared.Api.Account.Wants.Cards;
using LocalTrader.Shared.Api.Account.Wants.WantLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Account.Wants.Cards;

public abstract class WantedCard
{
    public WantedCardId Id { get; private init; }
    
    public required UserId UserId { get; init; }
    public User? User { get; init; }
    
    public required WantListId WantListId { get; init; }
    public WantList? WantList { get; init; }
    
    public required CardCondition MinimumCondition { get; set; }
    public required int Quantity { get; set; }
}

internal sealed class WantedCardConfiguration : IEntityTypeConfiguration<WantedCard>
{
    public void Configure(EntityTypeBuilder<WantedCard> builder)
    {
        builder.ToTable("WantedCards", Schemas.Account);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}