using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

/// <summary>
///     Generate appointments based on dto
/// </summary>
/// <param name="dto"></param>
public class GenerateAppointmentsRequest(GenerateAppointmentsDto dto) : Request<Response>
{
    /// <summary>
    ///     Generate appoinments dto
    /// </summary>
    public GenerateAppointmentsDto Dto { get; set; } = dto;
}
