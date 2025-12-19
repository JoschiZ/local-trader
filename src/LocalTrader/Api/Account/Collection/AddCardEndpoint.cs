using FastEndpoints;
using LocalTrader.Shared.Api.Account.Collection;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LocalTrader.Api.Account.Collection;


public class AddCardEndpoint : Endpoint<AddCardRequest, Created>
{
    public override void Configure()
    {
        Get("collection/add-card");
    }

    public override async Task<Created> ExecuteAsync(AddCardRequest req, CancellationToken ct)
    {
        return TypedResults.Created();
    }
}