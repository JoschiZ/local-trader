using LocalTrader.Data.Account;
using LocalTrader.Shared.Api.Account.Users;
using LocalTrader.Shared.Api.Account.Wants.WantLists;
using LocalTrader.Shared.Api.Magic.Wants.Lists;
using LocalTrader.Shared.Data.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalTrader.Data.Magic;

public sealed class MagicWantList
{
    public MagicWantListId Id { get; private init; }
    public required UserId UserId { get; init; }
    public User? User { get; init; }
    
    public required string Name { get; set; }
    public required Accessibility Accessibility { get; set; }

}

internal sealed class MagicWantListConfiguration : IEntityTypeConfiguration<MagicWantList>
{
    public void Configure(EntityTypeBuilder<MagicWantList> builder)
    {
        builder.ToTable("WantLists", Schemas.Magic);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}