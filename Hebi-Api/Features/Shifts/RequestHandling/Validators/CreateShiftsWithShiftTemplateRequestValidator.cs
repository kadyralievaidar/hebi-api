using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;

namespace Hebi_Api.Features.Shifts.RequestHandling.Validators;
public class CreateShiftsWithShiftTemplateRequestValidator : AbstractValidator<CreateShiftsWithShiftTemplateRequest>
{
    public CreateShiftsWithShiftTemplateRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.Dto.ShiftTemplateId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.ShiftTemplateRepository.AnyAsync(x => x.Id == id))
            .WithMessage("Choose the correct shift template");

        RuleFor(r => r.Dto).Must(x => x.StartDate < x.EndDate).WithMessage("Start date can't be earlier than End date");
    }
}
