using FluentValidation;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Appointments.RequestHandling.Validators;

public class GetAppointmentByIdRequestValidator : AbstractValidator<GetAppoitmentByIdRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAppointmentByIdRequestValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(x => x.AppointmnetId).MustAsync(async (appointmentId, cancellationToken) => 
                                                await _unitOfWork.AppointmentRepository.ExistAsync(appointmentId))
                                                .WithMessage("Appointment doesn't exist");
    }
}
