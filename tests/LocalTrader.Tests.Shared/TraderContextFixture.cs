using LocalTrader.Data;
using LocalTrader.Data.Account;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Testcontainers.Xunit;
using Xunit.Sdk;

namespace LocalTrader.Tests.Shared;

public class TraderContextFixture : ContainerFixture<PostgreSqlBuilder, PostgreSqlContainer>
{
    public static UserId TestUserId { get; } = UserId.Parse("019b861b-e4d3-7a68-a5ff-eba3c175fa6c");
    
    public TraderContextFixture(IMessageSink messageSink) : base(messageSink)
    {
    }

    protected override PostgreSqlBuilder Configure()
    {
        return new PostgreSqlBuilder("postgres:17.6");
    }

    public TraderContext GetContextInTransaction()
    {
        var connection = Container.GetConnectionString();

        var options = new DbContextOptionsBuilder<TraderContext>()
            .UseNpgsql(connection)
            .Options;
        
        var context = new TraderContext(options);
        context.Database.EnsureCreated();

        if (!context.Users.Any(x => x.Id == TestUserId))
        {
            context.Database.ExecuteSql($"""
                                        INSERT INTO "AspNetUsers" (
                                            "Id",
                                            "DisplayName",
                                            "EmailConfirmed",
                                            "PhoneNumberConfirmed",
                                            "TwoFactorEnabled",
                                            "LockoutEnabled",
                                            "AccessFailedCount",
                                            "Location_Longitude",
                                            "Location_Latitude",
                                            "Location_Radius")
                                        SELECT
                                            {TestUserId.Value},
                                            'Static Test User',
                                            true,
                                            true,
                                            false,
                                            false,
                                            0,
                                            53.57074108769329,
                                            9.984302847504159,
                                            5_000
                                        """);
        }
        
        context.Database.BeginTransaction();
        return context;
    } 
}

