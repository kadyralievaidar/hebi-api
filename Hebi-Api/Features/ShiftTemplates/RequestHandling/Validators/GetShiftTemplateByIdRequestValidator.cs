using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Validators;

public class GetShiftTemplateByIdRequestValidator : AbstractValidator<GetShiftTemplateByIdRequest>
{
    public GetShiftTemplateByIdRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.ShiftTemplateId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.ShiftTemplateRepository.AnyAsync(x => x.Id == id))
            .WithMessage("Shift template with provided Id doesn't exist");
    }
}
