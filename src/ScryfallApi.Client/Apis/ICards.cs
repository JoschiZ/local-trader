using ScryfallApi.Client.Models;
using static ScryfallApi.Client.Models.SearchOptions;

namespace ScryfallApi.Client.Apis;

/// <summary>
/// APIs for cards. Card objects represent individual Magic: The Gathering cards that players could
/// obtain and add to their collection (with a few minor exceptions).
/// </summary>
public interface ICards
{
    /// <summary>
    /// Fetch a card at random.
    /// </summary>
    /// <returns></returns>
    Task<ScryfallResult<Card>> GetRandom();
    
    /// <summary>
    /// Search for cards with a sort option
    /// </summary>
    /// <param name="query"></param>
    /// <param name="page"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    Task<ScryfallResult<ResultList<Card>>> Search(string query, int page, CardSort sort);

    /// <summary>
    /// Search for cards using the full search options available
    /// </summary>
    /// <param name="query"></param>
    /// <param name="page"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<ScryfallResult<ResultList<Card>>> Search(string query, int page, SearchOptions options);

    /// <summary>
    /// Searches a card by name
    /// </summary>
    /// <param name="cardName">the full or partial card name</param>
    /// <param name="fuzzy">if true, the search will be fuzzy by card name</param>
    /// <param name="setCode">a set code restriction</param>
    /// <returns></returns>
    Task<ScryfallResult<Card>> Named(string cardName, bool fuzzy, string? setCode = null);
    
    Task<ScryfallResult<Card>> ById(Guid id);
}
