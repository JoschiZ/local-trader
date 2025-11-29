using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LocalTrader.Api.Account.Manage;

internal sealed class SetLocationEndpoint : EndpointWithoutRequest<Ok>
{
    public override void Configure()
    {
        Get("/Test");
    }

    public override Task<Ok> ExecuteAsync(CancellationToken ct)
    {
        return Task.FromResult(TypedResults.Ok());
    }
}