using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.Dtos;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

/// <summary>
///     Get paged list of shift templates
/// </summary>
public class GetShiftTemplatesRequest : Request<Response>
{
    /// <summary>
    ///     Shift template's Id
    /// </summary>
    public GetPagedListOfShiftsTemplatesDto Dto { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="dto"></param>
    public GetShiftTemplatesRequest(GetPagedListOfShiftsTemplatesDto dto)
    {
        Dto = dto;
    }
}
