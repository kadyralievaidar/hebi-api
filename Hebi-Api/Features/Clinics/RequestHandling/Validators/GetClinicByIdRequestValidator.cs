using FluentValidation;
using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Clinics.RequestHandling.Validators;

public class GetClinicByIdRequestValidator : AbstractValidator<GetClinicByIdRequest>
{
    public GetClinicByIdRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(c => c.ClinicId).MustAsync(async (clinicId, cancellationToken) =>
                    await unitOfWork.ClinicRepository.AnyAsync(x => x.Id == clinicId))
                    .WithMessage("There is not clinic with this Id");
    }
}
