using FluentValidation;
using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Clinics.RequestHandling.Validators;

public class CreateClinicRequestValidator : AbstractValidator<CreateClinicRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateClinicRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(r => r.CreateClinicDto.Name)
            .NotEmpty().WithMessage("Clinic name is required.")
            .MustAsync(BeUniqueName).WithMessage("Clinic name must be unique.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _unitOfWork.ClinicRepository.AnyAsync(x => x.Name == name);
    }
}
