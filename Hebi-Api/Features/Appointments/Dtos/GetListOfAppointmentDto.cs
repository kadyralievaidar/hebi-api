using Hebi_Api.Features.Core.DataAccess.Models;
using System.ComponentModel;

namespace Hebi_Api.Features.Appointments.Dtos;

/// <summary>
///     Get list of appointments dto
/// </summary>
public class GetListOfAppointmentDto
{
    /// <summary>
    ///     Start date time
    /// </summary>
    public DateTime? StartDate { get; set; } =  DateTime.MinValue;

    /// <summary>
    ///     End date time
    /// </summary>
    public DateTime? EndDate { get; set; } =  DateTime.MaxValue;

    /// <summary>
    ///     Patient's id
    /// </summary>
    public Guid? PatientId { get; set; }

    /// <summary>
    ///     Shift's id
    /// </summary>
    public Guid? ShiftId { get; set; }

    /// <summary>
    ///     Search text
    /// </summary>
    public string? SearchText { get; set; }

    /// <summary>
    ///     Sort by 
    /// </summary>
    public string? SortBy { get; set; } = nameof(Appointment.CreatedAt);

    /// <summary>
    ///     Sort direction
    /// </summary>
    public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;
}
