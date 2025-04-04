using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;

namespace Hebi_Api.Features.Shifts.RequestHandling.Validators;

public class GetShiftByIdRequestValidator : AbstractValidator<GetShiftByIdRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetShiftByIdRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(c => c.ShiftId).MustAsync(async (shiftId, cancellationToken) => await _unitOfWork.ShiftsRepository.ExistAsync(shiftId)).
            WithMessage("The shift with this doesn't exist");
    }
}
