using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;

namespace Hebi_Api.Features.Shifts.RequestHandling.Validators;

/// <summary>
///     Assign shift request's validator
/// </summary>
public class AssignShiftRequestValidator : AbstractValidator<AssignShiftRequest>
{
    public AssignShiftRequestValidator(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        RuleFor(r => r)
            .MustAsync(async (request, cancellation) =>
                await CheckAssignedShifts(request, unitOfWork, contextAccessor))
            .WithMessage("Doctor can not be assigned.");
    }
    private async Task<bool> CheckAssignedShifts(
            AssignShiftRequest request,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor)
    {
        var userId = request.DoctorId ?? contextAccessor.GetUserIdentifier();
        var shift = await unitOfWork.ShiftsRepository.FirstOrDefaultAsync(x => x.Id == request.ShiftId);

        if (shift == null)
            return false;

        var hasOverlap = await unitOfWork.ShiftsRepository.AnyAsync(x =>
            x.DoctorId.HasValue &&
            x.DoctorId == userId &&
            x.Id != shift.Id && 
            x.StartTime < shift.EndTime &&
            x.EndTime > shift.StartTime);

        return !hasOverlap;
    }
}
