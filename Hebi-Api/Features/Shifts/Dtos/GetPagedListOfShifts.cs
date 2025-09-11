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
    public DateOnly StartDate { get; set; } = DateOnly.MinValue;

    /// <summary>
    ///     End date time
    /// </summary>
    public DateOnly EndDate { get; set; } = DateOnly.MaxValue;

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
