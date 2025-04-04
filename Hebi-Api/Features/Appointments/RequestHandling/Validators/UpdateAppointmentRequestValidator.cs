using FluentValidation;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Appointments.RequestHandling.Validators;

public class UpdateAppointmentRequestValidator : AbstractValidator<UpdateAppointmentRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAppointmentRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(c => c.AppointmentId).MustAsync(async (appointmentId, cancellationToken)
                => await _unitOfWork.AppointmentRepository.ExistAsync(appointmentId)).
                WithMessage("There is not clinic with this Id");
    }
}
