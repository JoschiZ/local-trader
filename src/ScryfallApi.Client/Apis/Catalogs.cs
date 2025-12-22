using ScryfallApi.Client.Models;

namespace ScryfallApi.Client.Apis;

///<inheritdoc cref="ICatalogs"/>
public class Catalogs : ICatalogs
{
    private readonly BaseRestService _restService;

    internal Catalogs(BaseRestService restService)
    {
        _restService = restService;
    }
    public async Task<ScryfallResult<string[]>> ListCardNames() => (await _restService.GetAsync<Catalog>("/catalog/card-names").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListWordBank() => (await _restService.GetAsync<Catalog>("/catalog/word-bank").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListCreatureTypes() => (await _restService.GetAsync<Catalog>("/catalog/creature-types").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListPlaneswalkerTypes() => (await _restService.GetAsync<Catalog>("/catalog/planeswalker-types").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListLandTypes() => (await _restService.GetAsync<Catalog>("/catalog/land-types").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListSpellTypes() => (await _restService.GetAsync<Catalog>("/catalog/spell-types").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListEnchantmentTypes() => (await _restService.GetAsync<Catalog>("/catalog/enchantment-types").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListArtifactTypes() => (await _restService.GetAsync<Catalog>("/catalog/artifact-types").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListPowers() => (await _restService.GetAsync<Catalog>("/catalog/powers").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListToughnesses() => (await _restService.GetAsync<Catalog>("/catalog/toughnesses").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListLoyalties() => (await _restService.GetAsync<Catalog>("/catalog/loyalties").ConfigureAwait(false)).Bind(x => x.Data);
    public async Task<ScryfallResult<string[]>> ListWatermarks() => (await _restService.GetAsync<Catalog>("/catalog/watermarks").ConfigureAwait(false)).Bind(x => x.Data);
}
