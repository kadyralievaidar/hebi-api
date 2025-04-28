using FluentValidation;
using FluentValidation.Results;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Core.Common.Validators
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : class
    {
        private readonly IValidator<TRequest> _compositeValidator;

        public ValidationPipelineBehavior(IValidator<TRequest> compositeValidator)
        {
            _compositeValidator = compositeValidator;
        }
        public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _compositeValidator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return await next();
            }
            if(!((object)request is Request request2))
            {
                return await next();
            }
            return Response.BadRequest(request2.Id, new ApplicationException("Validation error"), validationResult.Errors.DistinctBy((x) => x.PropertyName).ToDictionary((kvp) => kvp.PropertyName, (kvp) => kvp.ErrorMessage)) as TResponse;
        }
    }
}
