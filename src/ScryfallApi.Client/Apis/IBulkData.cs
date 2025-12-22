using ScryfallApi.Client.Models;

namespace ScryfallApi.Client.Apis;

/// <summary>Bulk Data API</summary>
public interface IBulkData
{
    /// <summary>
    /// Get information on bulk data collections and download links
    /// </summary>
    /// <returns></returns>
    Task<ScryfallResult<ResultList<BulkDataItem>>> Get();

    /// <summary>
    /// Return just the items that have been updated since the date and time provided
    /// </summary>
    /// <param name="updatedSince"></param>
    /// <returns></returns>
    Task<ScryfallResult<List<BulkDataItem>>> Get(DateTimeOffset updatedSince);

    /// <summary>
    /// Return just the item type specified that has been updated since the date and time provided
    /// </summary>
    /// <param name="updatedSince"></param>
    /// <param name="bulkDataType"></param>
    /// <returns></returns>
    Task<ScryfallResult<BulkDataItem>> Get(DateTimeOffset updatedSince, string bulkDataType);
}
