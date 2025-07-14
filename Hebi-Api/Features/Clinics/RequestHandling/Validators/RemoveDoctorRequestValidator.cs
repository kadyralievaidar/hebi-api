using FluentValidation;
using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;
using System.Linq;

namespace Hebi_Api.Features.Clinics.RequestHandling.Validators;

/// <summary>
///     Remove doctor request validator
/// </summary>
public class RemoveDoctorRequestValidator : AbstractValidator<RemoveDoctorsRequest>
{
    public RemoveDoctorRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.DoctorIds).NotEmpty().WithMessage("The doctor ids should not be empty");
        RuleFor(r => r.DoctorIds)
                   .MustAsync(async (doctorIds, cancellationToken) =>
                   {
                       var doctorsInDb = (await unitOfWork.UsersRepository.WhereAsync(x => doctorIds.Contains(x.Id)))
                                         .Select(x => x.Id).ToHashSet();

                       return doctorIds.All(id => doctorsInDb.Contains(id));
                   })
                   .WithMessage("Some doctor ids are invalid");
    }
}
