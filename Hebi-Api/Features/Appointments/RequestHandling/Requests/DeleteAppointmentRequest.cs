using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

/// <summary>
///     Delete appointment request
/// </summary>
public class DeleteAppointmentRequest : Request<Response>
{
    /// <summary>
    ///     Appointment id
    /// </summary>
    public Guid AppointmentId { get; set; }

    public DeleteAppointmentRequest(Guid appointmentId)
    {
        AppointmentId = appointmentId;
    }
}
