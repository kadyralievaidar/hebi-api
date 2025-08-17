using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.Dtos;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

/// <summary>
///     Create shift template request
/// </summary>
public class CreateShiftTemplateRequest : Request<Response>
{
    /// <summary>
    ///     Create shift dto
    /// </summary>
    public CreateShiftTemplateDto Dto { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="dto"></param>
    public CreateShiftTemplateRequest(CreateShiftTemplateDto dto)
    {
        Dto = dto;
    }
}
