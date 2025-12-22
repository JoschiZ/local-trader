using LocalTrader.Data.Account;
using LocalTrader.Data.Cards.Magic;
using LocalTrader.Shared.Api.Account.Collections;
using LocalTrader.Shared.Api.Account.Users;
using LocalTrader.Shared.Data.Cards.Magic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Api.Account.Collections;

/// <summary>
/// Represents generic card in a users collection across multiple games
/// </summary>
public abstract class CollectionCard
{
    public CollectionCardId Id { get;  private init; }
    public required UserId UserId { get; init; }
    public User? User { get; init; }
    public required string Name { get; init; }
    public required CardCondition Condition { get; init; }
    public required int Quantity { get; init; }
}

internal sealed class CollectionCardConfig : IEntityTypeConfiguration<CollectionCard>
{
    public void Configure(EntityTypeBuilder<CollectionCard> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.User).WithMany(x => x.Collection).HasForeignKey(x => x.UserId);
        builder.Property(x => x.Name).HasMaxLength(100);
    }
}

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