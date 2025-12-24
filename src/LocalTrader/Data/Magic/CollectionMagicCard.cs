using LocalTrader.Data.Account;


using LocalTrader.Shared.Data.Account;

using LocalTrader.Shared.Data.Magic.Cards;
using LocalTrader.Shared.Data.Magic.Collection;
using LocalTrader.Shared.Data.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Magic;

public sealed class CollectionMagicCard
{
    public CollectionMagicCardId Id { get; private init; }
    public required UserId UserId { get; init; }
    public User? User { get; init; }
    public required CardCondition Condition { get; init; }
    public required int Quantity { get; set; }
    public required MagicCardId CardId { get; init; }
    public MagicCard? Card { get; init; }
}

internal sealed class CollectionMagicCardConfiguration : IEntityTypeConfiguration<CollectionMagicCard>
{
    public void Configure(EntityTypeBuilder<CollectionMagicCard> builder)
    {
        builder.ToTable("CollectionCards", Schemas.Magic);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.User).WithMany(x => x.MagicCollection).HasForeignKey(x => x.UserId);
        
        builder
            .HasOne(x => x.Card)
            .WithMany(x => x.Collections)
            .HasForeignKey(x => x.CardId);

        builder
            .HasIndex(x => new { x.UserId, x.CardId, x.Condition })
            .IsUnique();
    }
}