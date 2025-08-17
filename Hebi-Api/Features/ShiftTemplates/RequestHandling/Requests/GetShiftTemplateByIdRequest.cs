using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

/// <summary>
///     Get shift template by id
/// </summary>
public class GetShiftTemplateByIdRequest : Request<Response>
{
    /// <summary>
    ///     Shift template's id
    /// </summary>
    public Guid ShiftTemplateId { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="shiftTemplateId"></param>
    public GetShiftTemplateByIdRequest(Guid shiftTemplateId)
    {
        ShiftTemplateId = shiftTemplateId;
    }
}
