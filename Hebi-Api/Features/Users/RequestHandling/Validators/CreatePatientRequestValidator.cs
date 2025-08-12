using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Microsoft.Extensions.Localization;

namespace Hebi_Api.Features.Users.RequestHandling.Validators;

public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
{
    public CreatePatientRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.Dto.PhoneNumber).NotEmpty();
        RuleFor(r => r.Dto.PhoneNumber)
            .MustAsync(async (phoneNumber, cancellationToken) =>
            {
                var user = await unitOfWork.UsersRepository
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

                return user == null;
            })
            .WithMessage("Пользователь с таким номером телефона уже существует.");
    }
}
