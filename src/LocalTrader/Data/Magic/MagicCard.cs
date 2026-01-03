using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Magic;

public class MagicCard
{
    public MagicCardId Id { get; init; }
    public required string Name { get; init; }
    public required Uri ScryfallUrl { get; init; }
    public required ScryfallId ScryfallId { get; init; }
    public List<CollectionMagicCard> Collections { get; init; } = [];
}

internal sealed class MagicCardConfiguration : IEntityTypeConfiguration<MagicCard>
{
    public void Configure(EntityTypeBuilder<MagicCard> builder)
    {
        builder.ToTable("Cards", Schemas.Magic);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ScryfallUrl).IsRequired().HasMaxLength(100);
    }
}