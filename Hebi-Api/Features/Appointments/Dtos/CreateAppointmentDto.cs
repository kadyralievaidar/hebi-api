namespace Hebi_Api.Features.Appointments.Dtos;


/// <summary>
///     Create appointment dto
/// </summary>
public class CreateAppointmentDto
{
    /// <summary>
    ///     Start date time of appointment
    /// </summary>
    public DateTime StartDateTime { get; set; }

    /// <summary>
    ///     End date time of appointment
    /// </summary>
    public DateTime EndDateTime { get; set; }

    /// <summary>
    ///     Doctor's id
    /// </summary>
    public Guid? DoctorId { get; set; }

    /// <summary>
    ///     Patient's id
    /// </summary>
    public Guid? PatientId { get; set; }

    /// <summary>
    ///     If there isn't patient
    /// </summary>
    public string? ShortName { get; set; }
}
