using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.Dtos;

namespace Hebi_Api.Features.ShiftTemplates.Services;

public interface IShiftTemplateService
{
    /// <summary>
    ///     Create a shift template using shift template dto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task CreateShiftTemplate(CreateShiftTemplateDto dto);

    /// <summary>
    ///     Delete shift template 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteShiftTemplate(Guid id);

    /// <summary>
    ///     Update the shift template
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tempalate"></param>
    /// <returns></returns>
    Task UpdateShiftTempalate(Guid id, CreateShiftTemplateDto tempalate);

    /// <summary>
    ///     Get shift template by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ShiftTemplateDto>  GetShiftTemplateById(Guid id);

    /// <summary>
    ///     Get paged list of shift templates
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagedResult<ShiftTemplateDto>> GetShiftTemplates(GetPagedListOfShiftsTemplatesDto dto);

}
