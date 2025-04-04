using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;

namespace Hebi_Api.Features.Diseases.RequestHandling.Validators;

public class GetDiseaseByIdRequestValidator : AbstractValidator<GetDiseaseByIdRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDiseaseByIdRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(c => c.DiseaseId).MustAsync(async (diseaseId, cancellationToken) 
                                => await _unitOfWork.DiseaseRepository.ExistAsync(diseaseId)).
                                WithMessage("The disease with this id doesn't exist");
    }
}
