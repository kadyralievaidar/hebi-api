using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Hebi_Api.Features.Users.RequestHandling.Validators;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        RuleFor(x => x.Request.Username)
            .NotEmpty().WithMessage("Username cannot be empty")
            .MustAsync(async (username, cancellationToken) =>
            {
                var user = await unitOfWork.UsersRepository.FirstOrDefaultAsync(x =>
                    x.NormalizedUserName == username.ToUpperInvariant());
                return user != null;
            }).WithMessage("User not found");

        RuleFor(x => x)
            .MustAsync(async (x, cancellationToken) =>
            {
                var username = x.Request.Username;
                var password = x.Request.Password;

                var user = await unitOfWork.UsersRepository.FirstOrDefaultAsync(u =>
                    u.NormalizedUserName == username.ToUpperInvariant());

                return user != null && await userManager.CheckPasswordAsync(user, password);
            }).WithMessage("Incorrect username or password");
    }
}