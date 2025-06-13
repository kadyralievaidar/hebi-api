using FluentValidation.TestHelper;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.RequestHandling.Validators;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.Appoitments.Validators;
[TestFixture]
public class GetAppointmentByIdRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private GetAppointmentByIdRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _validator = new GetAppointmentByIdRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_Appointment_Does_Not_Exist()
    {
        // Arrange
        var request = new GetAppoitmentByIdRequest(Guid.NewGuid());

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AppointmnetId)
              .WithErrorMessage("Appointment doesn't exist");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Appointment_Exists()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var doctorId= Guid.NewGuid();
        var shiftId = Guid.NewGuid();
        _dbFactory.AddData(new List<ApplicationUser>() { new()
        {
            Id = doctorId,
            UserName = "Test",
            FirstName = "Test",
            LastName = "Test",
            ClinicId = TestHelper.ClinicId
        } });
        _dbFactory.AddData(new List<Shift>() { new()
        {
            Id = shiftId
        } });
        _dbFactory.AddData(new List<Appointment>
        {
            new Appointment
            {
                Id = appointmentId,
                DoctorId = doctorId,
                StartDate = new DateTime(2025, 5, 5, 9, 0, 0),
                EndDate = new DateTime(2025, 5, 5, 10, 0, 0),
                ClinicId = TestHelper.ClinicId,
                Name = "test",
                Description = "test",
                ShiftId = shiftId
            }
        });

        var request = new GetAppoitmentByIdRequest(appointmentId);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AppointmnetId);
    }
}
