using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

public class DeleteAppointmentRequest : Request<Response>
{
    public Guid AppointmentId { get; set; }
}
