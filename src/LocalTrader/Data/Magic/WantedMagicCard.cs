using LocalTrader.Shared.Api.Magic.Wants.Cards;
using LocalTrader.Shared.Api.Magic.Wants.Lists;

using LocalTrader.Shared.Data.Magic.Cards;
using LocalTrader.Shared.Data.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Magic;

public sealed class WantedMagicCard
{
    public WantedMagicCardId Id { get; private init; }
    public required CardCondition MinimumCondition { get; set; }
    public required int Quantity { get; set; }
    
    public required MagicWantListId WantListId { get; init; }
    public MagicWantList? WantList { get; private init; }
    
    public required MagicCardId CardId { get; init; }
    public MagicCard? Card { get; private init; }
}

internal sealed class WantedMagicCardConfiguration : IEntityTypeConfiguration<WantedMagicCard>
{
    public void Configure(EntityTypeBuilder<WantedMagicCard> builder)
    {
        builder.ToTable("WantedCards", Schemas.Magic);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(x => new { x.CardId, x.WantListId }).IsUnique();
        builder.HasOne(x => x.Card).WithMany().HasForeignKey(x => x.CardId);
        builder.HasOne(x => x.WantList).WithMany(x => x.Cards).HasForeignKey(x => x.WantListId);
    }
}