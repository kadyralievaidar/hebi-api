namespace Hebi_Api.Features.Appointments.Dtos;


/// <summary>
///     Create appointment dto
/// </summary>
public class CreateAppointmentDto
{
    /// <summary>
    ///     The name of appoitnment
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     The description of appoitment
    /// </summary>
    public string? Description { get; set; }
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
    ///     If there isn't patient
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    ///     File path
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    ///     Shift's id
    /// </summary>
    public Guid? ShiftId { get; set; }

    /// <summary>
    ///     User card's Id
    /// </summary>
    public Guid? UserCardId { get; set; }

    /// <summary>
    ///     Disease's id
    /// </summary>
    public Guid? DiseaseId { get; set; }
}
