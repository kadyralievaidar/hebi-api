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
        var transcation = _unitOfWork.BeginTransaction();
        try
        {

            var shift = new Shift()
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                DoctorId = dto.DoctorId ?? _contextAccessor.GetUserIdentifier()
            };

            await _unitOfWork.ShiftsRepository.InsertAsync(shift);
            await _unitOfWork.SaveAsync();

            if (dto.AppointmentIds.Any())
            {
                var appointments = await _unitOfWork.AppointmentRepository.SearchAsync(x => dto.AppointmentIds.Contains(x.Id));

                foreach (var appointment in appointments)
                    appointment.ShiftId = shift.Id;

                await _unitOfWork.AppointmentRepository.UpdateRangeAsync(appointments);
                await _unitOfWork.SaveAsync();
            }

            await transcation.CommitAsync();
            return shift.Id;
        }
        catch (Exception e)
        {
            await transcation.RollbackAsync();
            Console.WriteLine(e.Message);
            throw;
        }
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
        var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var shift = await _unitOfWork.ShiftsRepository.GetByIdAsync(id)
                ?? throw new NullReferenceException(nameof(Shift));

            var appointments = await _unitOfWork.AppointmentRepository.SearchAsync(x => dto.AppointmentIds.Contains(x.Id));

            shift.StartTime = dto.StartTime;
            shift.EndTime = dto.EndTime;
            shift.DoctorId = dto.DoctorId;

            if (appointments != null && appointments.Any())
            {
                foreach (var appointment in appointments!)
                    appointment.ShiftId = shift.Id;

                await _unitOfWork.AppointmentRepository.UpdateRangeAsync(appointments);
            }
            _unitOfWork.ShiftsRepository.Update(shift);
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
            return shift;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e.Message);
            throw;
        }
    }
    public async Task CreateShiftsWithShiftTemplate(CreateShiftsWithTemplateDto dto)
    {
        var template = await _unitOfWork.ShiftTemplateRepository.GetByIdAsync(dto.ShiftTemplateId) 
            ?? throw new ArgumentException("Shift template not found");

        var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var currentDate = dto.StartDate;
            while (currentDate <= dto.EndDate)
            {
                if (!dto.DayOfWeeks.Contains(currentDate.DayOfWeek))
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                var startDateTime = currentDate.ToDateTime(template.StartTime, DateTimeKind.Utc);
                var endDateTime = template.EndTime < template.StartTime
                    ? currentDate.AddDays(1).ToDateTime(template.EndTime, DateTimeKind.Utc)
                    : currentDate.ToDateTime(template.EndTime, DateTimeKind.Utc);

                var existingShift = await _unitOfWork.ShiftsRepository
                    .SearchAsync(s => s.DoctorId == dto.DoctorId &&
                        s.StartTime < endDateTime && s.EndTime > startDateTime);

                if (existingShift.Count == 0)
                {
                    var shiftDto = new CreateShiftDto
                    {
                        StartTime = startDateTime,
                        EndTime = endDateTime,
                        DoctorId = dto.DoctorId,
                        ShiftTemplateId = template.Id
                    };

                    await CreateShiftInternal(shiftDto);
                }

                currentDate = currentDate.AddDays(1);
            }

            await transaction.CommitAsync();
            await _unitOfWork.SaveAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    private async Task<Guid> CreateShiftInternal(CreateShiftDto dto)
    {
        var shift = new Shift()
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            DoctorId = dto.DoctorId,
            ShiftTemplateId = dto.ShiftTemplateId
        };

        await _unitOfWork.ShiftsRepository.InsertAsync(shift);
        await _unitOfWork.SaveAsync();

        if (dto.AppointmentIds.Any())
        {
            var appointments = await _unitOfWork.AppointmentRepository.SearchAsync(x => dto.AppointmentIds.Contains(x.Id));
            foreach (var appointment in appointments)
                appointment.ShiftId = shift.Id;

            await _unitOfWork.AppointmentRepository.UpdateRangeAsync(appointments);
            await _unitOfWork.SaveAsync();
        }

        return shift.Id;
    }

}
