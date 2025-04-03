using FluentValidation;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Microsoft.Extensions.Localization;

namespace Hebi_Api.Features.Appointments.RequestHandling.Validators;

/// <inheritdoc />
public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _commonLocalize;

    /// <inheritdoc />
    public CreateAppointmentValidator(IUnitOfWork unitOfWork, IStringLocalizer commonLocalize)
    {
        _unitOfWork = unitOfWork;
        _commonLocalize = commonLocalize;

        RuleFor(c => c.Dto.DoctorId).NotEmpty()
        .WithMessage(string.Format($"{commonLocalize["NotEmpty"]}", "DoctorId"));

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
