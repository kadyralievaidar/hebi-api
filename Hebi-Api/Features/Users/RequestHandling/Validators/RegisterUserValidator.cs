using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using System.Threading;
namespace Hebi_Api.Features.Users.RequestHandling.Validators
{
    public class RegisterUserValidator: AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(c => c.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("UserName is required")
                .MinimumLength(4).WithMessage("UserName must be at least 4 characters");


            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");


            RuleFor(c => c.ConfirmPassword)
                .NotEmpty().WithMessage("ConfirmPassword is required")
                .Equal(c => c.Password).WithMessage("Passwords do not match");

            RuleFor(c => c.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Last name is required");
        }
    }
}
