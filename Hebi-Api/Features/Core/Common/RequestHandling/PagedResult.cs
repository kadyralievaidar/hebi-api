namespace Hebi_Api.Features.Core.Common.RequestHandling;

/// <summary>
///     Paginated result
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedResult<T>
{
    /// <summary>
    ///     Result list page
    /// </summary>
    public List<T> Results { get; set; } = new List<T>();

    /// <summary>
    ///     Total count of records
    /// </summary>
    public long TotalCount { get; set; }
}