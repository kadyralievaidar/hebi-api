using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Validators;

public class UpdateShiftTemplateRequestValidator :AbstractValidator<UpdateShiftTemplateRequest>
{
    public UpdateShiftTemplateRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.ShiftTemplateId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.ShiftTemplateRepository.AnyAsync(x => x.Id == id))
            .WithMessage("Shift template with provided Id doesn't exist");

        RuleFor(r => r.Dto.Name)
            .MustAsync(async (request, name, cancellationToken) =>
                !await unitOfWork.ShiftTemplateRepository.AnyAsync(x => x.Id != request.ShiftTemplateId 
                                    && x.Name == name))
            .WithMessage("Shift template with this name already exist");
    }
}
