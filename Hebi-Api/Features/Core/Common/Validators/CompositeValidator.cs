using FluentValidation;
using FluentValidation.Results;

namespace Hebi_Api.Features.Core.Common.Validators
{
    public class CompositeValidator<T> : IValidator<T>, IValidator
    {
        private readonly IEnumerable<IValidator<T>> _validators;

        public CascadeMode CascadeMode { get; set; }
        public CompositeValidator(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators;
        }
        public ValidationResult Validate(IValidationContext context)
        {
            HashSet<ValidationResult> hashSet = new HashSet<ValidationResult>();
            foreach (IValidator<T> validator in _validators)
            {
                ValidationResult item = validator.Validate(context);
                hashSet.Add(item);
            }

            return new ValidationResult(hashSet.SelectMany((ValidationResult vr) => vr.Errors));
        }

        public async Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default(CancellationToken))
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();
            foreach (IValidator<T> validator in _validators)
            {
                validationResults.Add(await validator.ValidateAsync(context, cancellation));
            }

            return new ValidationResult(validationResults.SelectMany((ValidationResult vr) => vr.Errors));
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            throw new NotImplementedException();
        }

        public bool CanValidateInstancesOfType(Type type)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(T instance)
        {
            T instance2 = instance;
            return new ValidationResult((from f in _validators.Select((IValidator<T> v) => v.Validate(instance2)).SelectMany((ValidationResult validationResult) => validationResult.Errors)
                                         where f != null
                                         select f).ToList());
        }

        public async Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = default(CancellationToken))
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();
            foreach (IValidator<T> validator in _validators)
            {
                validationResults.Add(await validator.ValidateAsync(instance, cancellation));
            }

            return new ValidationResult(validationResults.SelectMany((ValidationResult vr) => vr.Errors));
        }
    }
}
