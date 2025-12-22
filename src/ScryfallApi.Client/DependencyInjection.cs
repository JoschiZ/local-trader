using Microsoft.Extensions.DependencyInjection;

namespace ScryfallApi.Client;

public static class DependencyInjection
{
    public static IHttpClientBuilder AddScryfallApiClient(this IServiceCollection services) =>
        AddScryfallApiClient(services, ScryfallApiClientConfig.GetDefault());

    public static IHttpClientBuilder AddScryfallApiClient(this IServiceCollection services, Action<ScryfallApiClientConfig> configure)
    {
        var clientConfig = ScryfallApiClientConfig.GetDefault();
        configure(clientConfig);
        return AddScryfallApiClient(services, clientConfig);
    }

    public static IHttpClientBuilder AddScryfallApiClient(this IServiceCollection services, ScryfallApiClientConfig clientConfig)
    {
        services.AddScoped(_ => clientConfig.Clone());
        var clientBuilder = services.AddHttpClient<ScryfallApiClient>(client =>
        {
            client.BaseAddress = clientConfig.ScryfallApiBaseAddress;
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Local-Trader/0.1");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        });

        return clientBuilder;
    }
}
