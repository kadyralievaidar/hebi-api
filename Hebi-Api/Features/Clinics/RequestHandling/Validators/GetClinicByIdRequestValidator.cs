using FluentValidation;
using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Clinics.RequestHandling.Validators;

public class GetClinicByIdRequestValidator : AbstractValidator<GetClinicByIdRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetClinicByIdRequestValidator(IUnitOfWork unitOfWork)
    {
        //_unitOfWork = unitOfWork;
        //RuleFor(c => c.ClinicId).MustAsync(async(clinicId, cancellationToken) => 
        //            await _unitOfWork.ClinicRepository.GetClinicById(clinicId) == null)
        //            .WithMessage("There is not clinic with this Id");
    }
}
