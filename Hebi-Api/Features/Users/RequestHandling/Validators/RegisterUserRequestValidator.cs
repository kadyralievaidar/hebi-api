using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.RequestHandling.Requests;

namespace Hebi_Api.Features.Users.RequestHandling.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(c => c.RegisterUserDto.UserName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("UserName is required")
            .MinimumLength(4).WithMessage("UserName must be at least 4 characters");


        RuleFor(c => c.RegisterUserDto.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");


        RuleFor(c => c.RegisterUserDto.ConfirmPassword)
            .NotEmpty().WithMessage("ConfirmPassword is required")
            .Equal(c => c.RegisterUserDto.Password).WithMessage("Passwords do not match");

        RuleFor(c => c.RegisterUserDto.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(c => c.RegisterUserDto.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(c => c.RegisterUserDto.LastName)
            .NotEmpty().WithMessage("Last name is required");

        RuleFor(c => c.RegisterUserDto.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?\d{7,12}$")
            .WithMessage("Phone number must be valid and contain 7–12 digits");

        RuleFor(c => c.RegisterUserDto.UserName)
            .MustAsync(async (userName, cancellationToken) =>
                !await unitOfWork.UsersRepository.AnyAsync(u => u.UserName == userName))
            .WithMessage("The user with this username already exists");

        RuleFor(c => c.RegisterUserDto.Email)
           .MustAsync(async (email, cancellationToken) =>
               !await unitOfWork.UsersRepository.AnyAsync(e => e.Email == email))
           .WithMessage("The user with this email already exists");
    }
}