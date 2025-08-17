using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

/// <summary>
///     Create shifts with template request
/// </summary>
public class CreateShiftsWithShiftTemplateRequest : Request<Response>
{
    /// <summary>
    ///     Create a shift s with shift template dto
    /// </summary>
    public CreateShiftsWithTemplateDto Dto { get; set; }

    /// <summary>
    ///     Ctor
    /// </summary>
    /// <param name="dto"></param>
    public CreateShiftsWithShiftTemplateRequest(CreateShiftsWithTemplateDto dto)
    {
        Dto = dto;
    }
}
