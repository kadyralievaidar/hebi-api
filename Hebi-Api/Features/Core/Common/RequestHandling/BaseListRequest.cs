using System.ComponentModel;

namespace Hebi_Api.Features.Core.Common.RequestHandling;

public abstract class BaseListRequest
{
    /// <summary>
    ///     Page index
    /// </summary>
    public int PageIndex { get; set; } = 0;

    /// <summary>
    ///     Page size
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     Sort by property
    /// </summary>
    public string SortBy { get; set; } = "CreatedAt";

    /// <summary>
    ///     Sort direction
    /// </summary>
    public ListSortDirection SortDirection { get; set; } 

    /// <summary>
    ///     Search property
    /// </summary>
    public string? SearchText { get; set; }
}
