using LocalTrader.Data.Cards.Magic;
using LocalTrader.Shared.Data.Cards.Magic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Account.Collections;

public sealed class CollectionMagicCard : CollectionCard
{
    public required MagicCardId CardId { get; init; }
    public MagicCard? Card { get; init; }
}

internal sealed class CollectionMagicCardConfiguration : IEntityTypeConfiguration<CollectionMagicCard>
{
    public void Configure(EntityTypeBuilder<CollectionMagicCard> builder)
    {
        builder
            .HasOne(x => x.Card)
            .WithMany(x => x.Collections)
            .HasForeignKey(x => x.CardId);

        builder
            .HasIndex(x => new { x.UserId, x.CardId, x.Condition })
            .IsUnique();
    }
}