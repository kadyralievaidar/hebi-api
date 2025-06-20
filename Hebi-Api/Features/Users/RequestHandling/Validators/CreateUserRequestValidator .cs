using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.RequestHandling.Requests;

namespace Hebi_Api.Features.Users.RequestHandling.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(c => c.CreateUserDto.RegisterDto.UserName)
                .MustAsync(async (userName, cancellationToken) =>
                    !await _unitOfWork.UsersRepository.AnyAsync(u => u.UserName == userName))
                .WithMessage("The user with this username already exists");

            RuleFor(c => c.CreateUserDto.RegisterDto.Email)
               .MustAsync(async (email, cancellationToken) =>
                   !await _unitOfWork.UsersRepository.AnyAsync(e => e.Email == email))
               .WithMessage("The user with this email already exists");
        }
    }
}