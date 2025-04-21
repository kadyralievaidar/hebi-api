namespace Hebi_Api.Features.Appointments.Dtos;

public class UpdateAppointmentDto
{
    /// <summary>
    ///     Appointment's name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Appointment's description
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    ///     Start date time
    /// </summary>
    public DateTime StartDateTime { get; set; }

    /// <summary>
    ///     End date time
    /// </summary>
    public DateTime EndDateTime { get; set; }

    /// <summary>
    ///     Doctor's id
    /// </summary>
    public Guid? DoctorId { get; set; }

    /// <summary>
    ///     Shift's id
    /// </summary>
    public Guid? ShiftId { get; set; }

    /// <summary>
    ///     Patient's id
    /// </summary>
    public Guid? PatientId { get; set; }
}
