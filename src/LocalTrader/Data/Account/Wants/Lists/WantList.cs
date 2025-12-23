using LocalTrader.Data.Account.Wants.Cards;
using LocalTrader.Shared.Api.Account.Users;
using LocalTrader.Shared.Api.Account.Wants.WantLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Account.Wants.Lists;

public sealed class WantList
{
    public WantListId Id { get; private init; }
    
    public required UserId UserId { get; init; }
    public User? User { get; init; }

    public List<WantedCard> WantedCards { get; init; } = [];
    
    public required string Name { get; set; }
    public required Accessibility Accessibility { get; set; }
}

internal sealed class WantListConfiguration : IEntityTypeConfiguration<WantList>
{
    public void Configure(EntityTypeBuilder<WantList> builder)
    {
        builder.ToTable("WantLists", Schemas.Account);
        builder.HasOne(x => x.User).WithMany(x => x.WantLists).HasForeignKey(x => x.UserId);
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}