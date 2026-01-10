using FastEndpoints;
using LocalTrader.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LocalTrader.Tests;

public static class FactoryCreateExtensions
{

    public static void AddDefaultTestServices(
        this DefaultHttpContext context,
        TraderContext dbContext,
        Action<IServiceCollection>? configureServices = null)
    {
        context.AddTestServices(x =>
        {
            x.AddScoped<TraderContext>(_ => dbContext);
            configureServices?.Invoke(x);
        });
    }
       
}