using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.Services;

public interface IShiftsService
{
    /// <summary>
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateShift(CreateShiftDto dto);

    /// <summary>
    ///     Delete shift
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteShift(Guid id);

    /// <summary>
    ///     Update shift 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Shift> UpdateShift(Guid id, CreateShiftDto dto);

    /// <summary>
    ///     Get list of shifts
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<List<Shift>> GetListOfShiftsAsync(GetPagedListOfShiftsDto dto);

    /// <summary>
    ///     Get shift by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Shift> GetShiftAsync(Guid id);

    /// <summary>
    ///     Generates shifts based on shift template
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task CreateShiftsWithShiftTemplate(CreateShiftsWithTemplateDto dto);
}
