using FluentValidation;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Appointments.RequestHandling.Validators;

/// <inheritdoc />
public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <inheritdoc />
    public CreateAppointmentValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(c => c).MustAsync(CheckAppointmentTime);
    }

    /// <inheritdoc />
    private async Task<bool> CheckAppointmentTime(CreateAppointmentRequest request, CancellationToken token)
    {
        var conflicts = await _unitOfWork.AppointmentRepository.AnyAsync(x =>
            x.DoctorId == request.Dto.DoctorId &&
            x.StartDate <= request.Dto.EndDateTime &&
            x.EndDate >= request.Dto.StartDateTime);

        if (conflicts)
            return false;

        return true;
    }
}
