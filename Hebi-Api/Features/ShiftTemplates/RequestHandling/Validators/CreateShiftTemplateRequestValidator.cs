using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Validators;

public class CreateShiftTemplateRequestValidator : AbstractValidator<CreateShiftTemplateRequest>
{
    public CreateShiftTemplateRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.Dto.Name)
            .MustAsync(async (name, cancellationToken) => !await unitOfWork.ShiftTemplateRepository.AnyAsync(x => x.Name == name))
            .WithMessage("Shift template with this name already exist");
    }
}
