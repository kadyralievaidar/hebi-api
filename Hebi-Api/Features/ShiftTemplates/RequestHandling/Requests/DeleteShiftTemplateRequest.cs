using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

/// <summary>
///     Delete shift template
/// </summary>
public class DeleteShiftTemplateRequest : Request<Response>
{
    /// <summary>
    ///     Shift template's id
    /// </summary>
    public Guid ShiftTemplateId { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="shiftTemplateId"></param>
    public DeleteShiftTemplateRequest(Guid shiftTemplateId)
    {
        ShiftTemplateId = shiftTemplateId;
    }
}
