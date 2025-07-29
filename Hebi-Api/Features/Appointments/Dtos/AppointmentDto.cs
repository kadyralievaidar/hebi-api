namespace Hebi_Api.Features.Appointments.Dtos;

/// <summary>
///     Appointment dto
/// </summary>
public class AppointmentDto
{
    /// <summary>
    ///     Start date time
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    ///     End date time
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    ///     Doctor's id
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    ///     Doctor's name
    /// </summary>
    public string? DoctorName { get; set; }

    /// <summary>
    ///     Service's name
    /// </summary>
    public string? DiseaseName { get; set; }

    /// <summary>
    ///     Final price
    /// </summary>
    public decimal? Price { get; set; }
}
