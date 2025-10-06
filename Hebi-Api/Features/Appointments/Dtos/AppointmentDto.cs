using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Appointments.Dtos;

/// <summary>
///     Appointment dto
/// </summary>
public class AppointmentDto
{
    /// <summary>
    ///     Appointment's id
    /// </summary>
    public Guid Id { get; set; }
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
    public Guid? DoctorId { get; set; }

    /// <summary>
    ///     Doctor's name
    /// </summary>
    public string? DoctorName { get; set; }

    /// <summary>
    ///     Service's name
    /// </summary>
    public string? DiseaseName { get; set; }

    /// <summary>
    ///     Patient's name
    /// </summary>
    public string? PatientName { get; set; }

    /// <summary>
    ///     Patient's id
    /// </summary>
    public Guid? PatientId { get; set; }

    /// <summary>
    ///     User card's id
    /// </summary>
    public Guid? UserCardId { get; set; }

    /// <summary>
    ///     Shift's id
    /// </summary>
    public Guid? ShiftId { get; set; }

    /// <summary>
    ///     Final price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    ///     Service's color
    /// </summary>

    public string Color { get; set; }

    public AppointmentDto()
    {
    }

    public AppointmentDto(Appointment appointment)
    {
        Id = appointment.Id;
        StartDate = appointment.StartDate;
        EndDate = appointment.EndDate;
        DoctorId = appointment.DoctorId;
        DoctorName = appointment.Doctor?.LastName + appointment.Doctor?.FirstName;
        DiseaseName = appointment.Disease?.Name;
        PatientName = appointment.Patient?.LastName + appointment.Patient?.FirstName;
        PatientId = appointment.PatientId;
        UserCardId = appointment.UserCardId;
        ShiftId = appointment.ShiftId;
        Price = appointment.Disease?.Price;
        Color = appointment.Disease?.Color;
    }
}
