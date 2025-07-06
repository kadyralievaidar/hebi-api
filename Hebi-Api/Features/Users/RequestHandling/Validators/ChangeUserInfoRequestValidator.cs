using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.RequestHandling.Requests;

namespace Hebi_Api.Features.Users.RequestHandling.Validators;

public class ChangeUserInfoRequestValidator : AbstractValidator<ChangeUserInfoRequest>
{
    public ChangeUserInfoRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.Dto.UserId).MustAsync(async (id, cancellationToken) =>
        {
            return await unitOfWork.UsersRepository.AnyAsync(x => x.Id == id);
        }).WithMessage("User not found");
    }
}
