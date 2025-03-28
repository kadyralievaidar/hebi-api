using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

public class CreateAppointmentRequest : Request<Response>
{
    public CreateAppointmentDto Dto { get; set; }

    public CreateAppointmentRequest(CreateAppointmentDto dto)
    {
        Dto = dto;
    }
}
