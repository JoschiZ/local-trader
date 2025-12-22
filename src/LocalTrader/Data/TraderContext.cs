using JetBrains.Annotations;
using LocalTrader.Api.Account.Collections;
using LocalTrader.Data.Account;
using LocalTrader.Data.Account.Collections;
using LocalTrader.Data.Cards.Magic;
using LocalTrader.Generated;
using LocalTrader.Shared.Api.Account.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocalTrader.Data;

[UsedImplicitly(ImplicitUseKindFlags.Assign)]
public class TraderContext : IdentityDbContext<User, IdentityRole<UserId>, UserId>
{
    public TraderContext(DbContextOptions<TraderContext> options) : base(options)
    {
        Collections = new CollectionsSet(this);
        Cards = new CardsSet(this);
    }
    public CardsSet Cards { get; }
    public CollectionsSet Collections { get; }
    
    
    public DbSet<MagicCard> MagicCards { get; private init; }
    public DbSet<CollectionCard> AllCollectionCards { get; private init; }
    public DbSet<CollectionMagicCard> CollectionMagicCards { get; private init; }

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

    protected TraderContext Context { get; init; }
}

public sealed class CardsSet : DbSubset
{
    public CardsSet(TraderContext context) : base(context)
    {
    }

    public DbSet<MagicCard> Magic => Context.MagicCards;
}

public sealed class CollectionsSet : DbSubset
{
    public DbSet<CollectionCard> All => Context.AllCollectionCards;
    public DbSet<CollectionMagicCard> Magic => Context.CollectionMagicCards;
    public CollectionsSet(TraderContext context) : base(context)
    {
    }
}