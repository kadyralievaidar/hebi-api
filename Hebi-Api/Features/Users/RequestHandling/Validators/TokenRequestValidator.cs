using FluentValidation;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.RequestHandling.Validators;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(r => r.Request.Username).NotNull().WithMessage("User name can't be null");
        RuleFor(r => r.Request.Username).NotEmpty().WithMessage("User name can't be empty");
        RuleFor(r => r.Request.Username)
            .MustAsync(async (username, cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(username))
                    return true;

                return await unitOfWork.UsersRepository.FirstOrDefaultAsync(x =>
                    x.NormalizedUserName == username.ToUpperInvariant()) != null;
            })
            .WithMessage("Incorrect user data");
    }
}
