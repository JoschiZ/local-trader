using LocalTrader.Shared.Api.Account.Collections;
using LocalTrader.Shared.Api.Account.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Account.Collections;

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
    public required int Quantity { get; set; }
}

internal sealed class CollectionCardConfig : IEntityTypeConfiguration<CollectionCard>
{
    public void Configure(EntityTypeBuilder<CollectionCard> builder)
    {
        builder.ToTable("Cards", "Collections");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.User).WithMany(x => x.Collection).HasForeignKey(x => x.UserId);
        builder.Property(x => x.Name).HasMaxLength(100);
    }
}

