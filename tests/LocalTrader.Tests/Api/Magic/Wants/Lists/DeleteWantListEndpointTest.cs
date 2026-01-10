using FastEndpoints;
using JetBrains.Annotations;
using LocalTrader.Api.Magic.Wants.Lists;
using LocalTrader.Data.Magic;
using LocalTrader.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LocalTrader.Tests.Api.Magic.Wants.Lists;

[TestSubject(typeof(DeleteWantListEndpoint))]
public class DeleteWantListEndpointTest : DbTestBase
{
    private readonly DeleteWantListEndpoint _endpoint;
    
    public DeleteWantListEndpointTest(TraderContextFixture fixture) : base(fixture)
    {
        _endpoint = Factory
            .Create<DeleteWantListEndpoint>(context =>
            {
                context.AddDefaultTestServices(DbContext);
            });
    }

    [Fact]
    public async Task Should_Delete_Endpoint()
    {
        var wantList = new MagicWantList
        {
            UserId = TraderContextFixture.TestUserId,
            Name = "SomeName",
            Accessibility = WantListAccessibility.Public
        };
        DbContext.Magic.WantLists.Add(wantList);
        await DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var request = new DeleteWantListEndpoint.Request
        {
            UserId = TraderContextFixture.TestUserId,
            Id = wantList.Id
        };
        var result = await _endpoint.ExecuteAsync(request, TestContext.Current.CancellationToken);
        result.Result.ShouldBeOfType<NoContent>();
        
        var stillExists = await DbContext.Magic.WantLists.AnyAsync(w => w.Id == wantList.Id, cancellationToken: TestContext.Current.CancellationToken);
        stillExists.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Report_NotFound()
    {
        var request = new DeleteWantListEndpoint.Request
        {
            Id = MagicWantListId.Empty
        };
        var result = await _endpoint.ExecuteAsync(request, TestContext.Current.CancellationToken);
        result.Result.ShouldBeOfType<NotFound>();
    }
}