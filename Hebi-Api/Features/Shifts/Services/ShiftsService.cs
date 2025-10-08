using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Shifts.Dtos;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<ShiftDto>> GetListOfShiftsAsync(GetListOfShiftsDto dto)
    {
        var shifts = await _unitOfWork.ShiftsRepository.SearchQuery(x => 
                                x.StartTime <= dto.EndDate.ToDateTime(TimeOnly.MaxValue).EnsureUtc() &&
                                x.EndTime >= dto.StartDate.ToDateTime(TimeOnly.MinValue).EnsureUtc() && 
                                (!dto.DoctorId.HasValue || x.DoctorId == dto.DoctorId), 
                                dto.SortBy, 
                                dto.SortDirection,
                                relations : [nameof(Shift.Doctor), nameof(Shift.Appointments)])
                                .Select(x => new ShiftDto(x)).ToListAsync();
        return shifts;
    }

    public async Task<ShiftDto> GetShiftAsync(Guid id)
    {
        var shift = await _unitOfWork.ShiftsRepository.GetByIdAsync(id, relations: [nameof(Shift.Doctor), nameof(Shift.Appointments)]);

        var shiftDto = new ShiftDto()
        {
            ShiftId = shift.Id,
            DoctorInfo = new BasicInfoDto(shift.Doctor),
            StartTime = shift.StartTime, 
            EndTime = shift.EndTime,
            Appointments = shift.Appointments.Select(appointment => new AppointmentDto(appointment))
        };
        return shiftDto;
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
    public async Task AssignShift(Guid? doctorId, Guid shiftId)
    {
        var userId = doctorId ?? _contextAccessor.GetUserIdentifier();
        var shift = await _unitOfWork.ShiftsRepository.FirstOrDefaultAsync(x => x.Id == shiftId);
        shift!.DoctorId = userId;
        _unitOfWork.ShiftsRepository.Update(shift);
        await _unitOfWork.SaveAsync();
    }
}
