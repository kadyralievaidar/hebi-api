using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

/// <summary>
///     Update appointment request
/// </summary>
public class UpdateAppointmentRequest : Request<Response>
{
    /// <summary>
    ///     Appointment id
    /// </summary>
    public Guid AppointmentId { get; set; }

    /// <summary>
    ///     UpdateAppointmentDto
    /// </summary>
    public UpdateAppointmentDto Dto { get; set; }

    public UpdateAppointmentRequest(Guid appointmentId, UpdateAppointmentDto dto)
    {
        AppointmentId = appointmentId;
        Dto = dto;
    }
}
