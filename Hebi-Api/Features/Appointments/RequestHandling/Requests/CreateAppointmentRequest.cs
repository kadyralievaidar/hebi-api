using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

/// <summary>
///     Create appointment request
/// </summary>
public class CreateAppointmentRequest : Request<Response>
{
    /// <summary>
    ///     CreateAppointmentDto
    /// </summary>
    public CreateAppointmentDto Dto { get; set; }

    public CreateAppointmentRequest(CreateAppointmentDto dto)
    {
        Dto = dto;
    }
}
