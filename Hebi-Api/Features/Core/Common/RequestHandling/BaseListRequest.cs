using System.ComponentModel;

namespace Hebi_Api.Features.Core.Common.RequestHandling;

public abstract class BaseListRequest
{
    /// <summary>
    ///     Page index
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    ///     Page size
    /// </summary>
    public int PageSize { get; set; }

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
