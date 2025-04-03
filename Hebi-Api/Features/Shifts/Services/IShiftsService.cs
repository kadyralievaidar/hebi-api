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

    Task DeleteShift(Guid id);

    Task<Shift> UpdateShift(Guid id, CreateShiftDto dto);

    Task<List<Shift>> GetListOfShiftsAsync(GetPagedListOfShiftsDto dto);

    Task<Shift> GetShiftAsync(Guid id);
}
