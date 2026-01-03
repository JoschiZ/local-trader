using System.Collections.Generic;
using LocalTrader.Api.Magic.Wants.Lists;
using LocalTrader.Data.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserId = LocalTrader.Data.Account.UserId;

namespace LocalTrader.Data.Magic;

public sealed class MagicWantList
{
    public MagicWantListId Id { get; private init; }
    public required UserId UserId { get; init; }
    public User? User { get; init; }
    
    public required string Name { get; set; }
    public required WantListAccessibility Accessibility { get; set; }
    public List<WantedMagicCard> Cards { get; init; } = [];
}

internal sealed class MagicWantListConfiguration : IEntityTypeConfiguration<MagicWantList>
{
    public void Configure(EntityTypeBuilder<MagicWantList> builder)
    {
        builder.ToTable("WantLists", Schemas.Magic);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}