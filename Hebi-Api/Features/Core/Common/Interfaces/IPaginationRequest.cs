namespace Hebi_Api.Features.Core.Common.Interfaces;

/// <summary>
///     Pagination information
/// </summary>
public interface IPaginationRequest
{
    /// <summary>
    ///     The number of elements per page
    /// </summary>
    int PageSize { get; set; }

    /// <summary>
    ///     Current page number. 
    ///     The page number starts at 0.
    /// </summary>
    int PageNumber { get; set; }
}
