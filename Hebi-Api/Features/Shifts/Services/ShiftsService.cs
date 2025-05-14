using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.Services;

public class ShiftsService : IShiftsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;

    public ShiftsService(IUnitOfWork unitOfWork,IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public async Task<Guid> CreateShift(CreateShiftDto dto)
    {
        var appointments = await _unitOfWork.AppointmentRepository.SearchAsync(x => dto.AppointmentIds.Contains(x.Id));
        var shift = new Shift()
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            DoctorId = dto.DoctorId ?? _contextAccessor.GetUserIdentifier(),
            Appointments = appointments
        };
        await _unitOfWork.ShiftsRepository.InsertAsync(shift);
        await _unitOfWork.SaveAsync();
        return shift.Id;
    }

    public async Task DeleteShift(Guid id)
    {
        var shift = await _unitOfWork.ShiftsRepository.GetByIdAsync(id)
            ?? throw new NullReferenceException(nameof(Shift));
        _unitOfWork.ShiftsRepository.Delete(shift);
        await _unitOfWork.SaveAsync();
    }

    public async Task<List<Shift>> GetListOfShiftsAsync(GetPagedListOfShiftsDto dto)
    {
        var shifts = await _unitOfWork.ShiftsRepository.SearchAsync(x => x.StartTime >= dto.StartTime 
                                && x.EndTime <= dto.EndTime, dto.SortBy, 
                                dto.SortDirection, dto.PageSize * dto.PageIndex, dto.PageSize);
        return shifts;
    }

    public async Task<Shift> GetShiftAsync(Guid id)
    {
        var shift = await _unitOfWork.ShiftsRepository.GetByIdAsync(id)
                            ?? throw new NullReferenceException(nameof(Shift));
        return shift;
    }

    public async Task<Shift> UpdateShift(Guid id, CreateShiftDto dto)
    {
        var shift = await _unitOfWork.ShiftsRepository.GetByIdAsync(id)
            ?? throw new NullReferenceException(nameof(Shift));

        var appointments = await _unitOfWork.AppointmentRepository.SearchAsync(x => dto.AppointmentIds.Contains(x.Id));

        shift = new Shift() 
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            DoctorId = dto.DoctorId,
            Appointments = appointments
        };
        _unitOfWork.ShiftsRepository.Update(shift);
        await _unitOfWork.SaveAsync();
        return shift;
    }
}
