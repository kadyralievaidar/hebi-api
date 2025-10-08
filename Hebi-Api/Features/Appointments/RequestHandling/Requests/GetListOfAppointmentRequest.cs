using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

/// <summary>
///     Get list of appointment 
/// </summary>
public class GetListOfAppointmentRequest : Request<Response>
{
    /// <summary>
    ///     Get ListOfAppointmentDto
    /// </summary>
    public GetListOfAppointmentDto Dto { get; set; }

    public GetListOfAppointmentRequest(GetListOfAppointmentDto dto)
    {
        Dto = dto;
    }
}
