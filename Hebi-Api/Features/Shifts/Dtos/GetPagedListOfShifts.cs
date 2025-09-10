using System.ComponentModel;

namespace Hebi_Api.Features.Shifts.Dtos;

/// <summary>
///     Get list of shifts
/// </summary>
public class GetListOfShiftsDto
{
    /// <summary>
    ///     Start date time
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.MinValue;

    /// <summary>
    ///     End date time
    /// </summary>
    public DateTime EndTime { get; set; } = DateTime.MaxValue;

    /// <summary>
    ///     Doctor's id
    /// </summary>
    public Guid? DoctorId { get; set; }

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
