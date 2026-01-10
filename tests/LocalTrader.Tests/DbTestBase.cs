using LocalTrader.Data;
using LocalTrader.Tests.Shared;

namespace LocalTrader.Tests;

public abstract class DbTestBase : IClassFixture<TraderContextFixture>, IDisposable
{
    protected TraderContext DbContext { get; }

    protected DbTestBase(TraderContextFixture fixture)
    {
        DbContext = fixture.GetContextInTransaction();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        DbContext.Dispose();
    }
}