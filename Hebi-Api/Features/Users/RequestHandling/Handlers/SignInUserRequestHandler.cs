using FluentValidation;
using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class SignInUserRequestHandler : IRequestHandler<RegisterUserRequest, Response>
{
    private readonly IUsersService _service;
    private readonly ILogger<SignInUserRequestHandler> _logger;
    private readonly IValidator<RegisterUserDto> _validator;

    public SignInUserRequestHandler(IUsersService service, ILogger<SignInUserRequestHandler> logger, IValidator<RegisterUserDto> validator)
    {
        _service = service;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Response> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request.RegisterUserDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                                                    .GroupBy(e => e.PropertyName)
                                                    .ToDictionary(
                                                        g => g.Key,
                                                        g => string.Join("; ", g.Select(e => e.ErrorMessage))
                                                    );

                return Response.BadRequest(request.Id, null, errors);
            }
            
            await _service.Register(request.RegisterUserDto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
