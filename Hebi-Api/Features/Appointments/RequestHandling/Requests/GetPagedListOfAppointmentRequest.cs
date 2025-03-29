using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

/// <summary>
///     Get list of appointment 
/// </summary>
public class GetPagedListOfAppointmentRequest : Request<Response>
{
    /// <summary>
    ///     GetPagedListOfAppointmentDto
    /// </summary>
    public GetPagedListOfAppointmentDto Dto { get; set; }

    public GetPagedListOfAppointmentRequest(GetPagedListOfAppointmentDto dto)
    {
        Dto = dto;
    }
}
