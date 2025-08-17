using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.Dtos;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

/// <summary>
///     Update shift template request
/// </summary>
public class UpdateShiftTemplateRequest : Request<Response>
{
    /// <summary>
    ///     Shift template's Id
    /// </summary>
    public Guid ShiftTemplateId { get; set; }

    /// <summary>
    ///     Shift template dto
    /// </summary>
    public CreateShiftTemplateDto Dto { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    public UpdateShiftTemplateRequest(Guid id, CreateShiftTemplateDto dto)
    {
        ShiftTemplateId = id;
        Dto = dto;
    }
}
