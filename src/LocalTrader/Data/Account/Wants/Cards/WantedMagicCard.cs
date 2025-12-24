using LocalTrader.Data.Cards.Magic;
using LocalTrader.Shared.Data.Cards.Magic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Account.Wants.Cards;

public sealed class WantedMagicCard : WantedCard
{
    public required MagicCardId CardId { get; init; }
    public MagicCard? Card { get; private init; }
}

internal sealed class WantedMagicCardConfiguration : IEntityTypeConfiguration<WantedMagicCard>
{
    public void Configure(EntityTypeBuilder<WantedMagicCard> builder)
    {
        builder.HasIndex(x => new { x.CardId, x.WantListId }).IsUnique();
        builder.HasOne(x => x.Card).WithMany().HasForeignKey(x => x.CardId);
    }
}