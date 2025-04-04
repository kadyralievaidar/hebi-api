using FluentValidation;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;

namespace Hebi_Api.Features.Appointments.RequestHandling.Validators;

public class GetPagedListOfAppointmentRequestValidator : AbstractValidator<GetPagedListOfAppointmentRequest>
{
    public GetPagedListOfAppointmentRequestValidator()
    {
        RuleFor(c => c.Dto.StartDate).GreaterThan(c => c.Dto.EndDate)
            .WithMessage("Start date time can't be greater than End date time");
    }
}
