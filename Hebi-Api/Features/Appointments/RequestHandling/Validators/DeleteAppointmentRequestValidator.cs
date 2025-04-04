using FluentValidation;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Appointments.RequestHandling.Validators;

public class DeleteAppointmentRequestValidator : AbstractValidator<DeleteAppointmentRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAppointmentRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(c => c.AppointmentId)
                .MustAsync(async (appointment, cancellation) => await _unitOfWork.AppointmentRepository.ExistAsync(appointment))
                            .WithMessage("The specified appointment does not exist.");
       
    }
}
