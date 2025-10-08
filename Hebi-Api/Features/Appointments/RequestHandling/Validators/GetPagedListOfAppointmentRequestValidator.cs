using FluentValidation;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;

namespace Hebi_Api.Features.Appointments.RequestHandling.Validators;

public class GetPagedListOfAppointmentRequestValidator : AbstractValidator<GetListOfAppointmentRequest>
{
    public GetPagedListOfAppointmentRequestValidator()
    {
        RuleFor(c => c.Dto.EndDate).GreaterThan(c => c.Dto.StartDate)
            .WithMessage("Start date time can't be greater than End date time");
    }
}
