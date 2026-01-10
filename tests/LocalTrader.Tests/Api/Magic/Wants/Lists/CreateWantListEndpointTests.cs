using FastEndpoints;
using JetBrains.Annotations;
using LocalTrader.Api.Magic.Wants.Lists;
using LocalTrader.Data.Account;
using LocalTrader.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LocalTrader.Tests.Api.Magic.Wants.Lists;

[TestSubject(typeof(CreateWantListEndpoint))]
public class CreateWantListEndpointTests : DbTestBase
{
    private readonly CreateWantListEndpoint _endpoint;

    public CreateWantListEndpointTests(TraderContextFixture fixture) : base(fixture)
    {
        _endpoint = Factory
            .Create<CreateWantListEndpoint>(context =>
            {
                context.AddDefaultTestServices(DbContext);
            });
    }

    [Fact]
    public async Task Should_Fail_On_Missing_Id()
    {
        var dto = new CreateWantListEndpoint.Request
        {
            Name = "TestName",
            Accessibility = WantListAccessibility.Public,
            UserId = UserId.New()
        };

        await _endpoint
            .ExecuteAsync(dto, TestContext.Current.CancellationToken)
            .ShouldThrowAsync<DbUpdateException>();
    }
    
    [Fact]
    public async Task Should_Create_List()
    {
        var dto = new CreateWantListEndpoint.Request
        {
            Name = "TestName",
            Accessibility = WantListAccessibility.Public,
            UserId = TraderContextFixture.TestUserId
        };

        var result = await _endpoint.ExecuteAsync(dto, TestContext.Current.CancellationToken);
        result.ShouldBeOfType<Created>();
    }
    
}