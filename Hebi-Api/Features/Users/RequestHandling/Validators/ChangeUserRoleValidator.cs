using FluentValidation;
using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.RequestHandling.Requests;

namespace Hebi_Api.Features.Users.RequestHandling.Validators;

/// <summary>
///     Change user's role request validator
/// </summary>
public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleRequest>
{
    public ChangeUserRoleValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.UserId)
                 .MustAsync(async (id, cancellationToken) =>
                 {
                     if (id == null)
                         return true;

                     var user = await unitOfWork.UsersRepository.GetByIdAsync(id.Value);
                     return user != null;
                 })
                 .WithMessage("User with the specified ID does not exist.");

        RuleFor(r => r.RoleName)
            .Must(role => new[]
            {
                Consts.SuperAdmin,
                Consts.Admin,
                Consts.Doctor,
                Consts.Individual,
                Consts.Patient
            }.Contains(role))
            .WithMessage("Invalid role name");
    }
}

