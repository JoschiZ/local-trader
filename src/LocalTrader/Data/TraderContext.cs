using JetBrains.Annotations;
using LocalTrader.Data.Account;
using LocalTrader.Data.Magic;
using LocalTrader.Generated;
using LocalTrader.Shared.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Data;

[UsedImplicitly(ImplicitUseKindFlags.Assign)]
public class TraderContext : IdentityDbContext<User, IdentityRole<UserId>, UserId>
{
    public TraderContext(DbContextOptions<TraderContext> options) : base(options)
    {
        Magic = new MagicSet(this);
    }

    public MagicSet Magic { get; }
    internal DbSet<CollectionMagicCard> CollectionMagicCards { get; init; }
    internal DbSet<WantedMagicCard> WantedMagicCards { get; init; }
    internal DbSet<MagicWantList> MagicWantLists { get; init; }
    internal DbSet<MagicCard> MagicCards { get; init; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(TraderContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.RegisterLocalTraderSharedStronglyTypedIds();
    }
}


public abstract class DbSubset
{
    protected DbSubset(TraderContext context)
    {
        Context = context;
    }

    protected TraderContext Context { get; }
}

public sealed class MagicSet : DbSubset
{
    public MagicSet(TraderContext context) : base(context)
    {
    }

    public DbSet<CollectionMagicCard> Collections => Context.CollectionMagicCards;
    public DbSet<WantedMagicCard> WantedCards => Context.WantedMagicCards;
    public DbSet<MagicWantList> WantLists => Context.MagicWantLists;
    public DbSet<MagicCard> Cards => Context.MagicCards;
}
