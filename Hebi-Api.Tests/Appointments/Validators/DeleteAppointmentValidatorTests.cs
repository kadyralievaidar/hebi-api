using FluentValidation.TestHelper;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.RequestHandling.Validators;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.Appoitments.Validators;

[TestFixture]
public class DeleteAppointmentRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private DeleteAppointmentRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        _validator = new DeleteAppointmentRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_Appointment_Does_Not_Exist()
    {
        // Arrange
        var nonExistentAppointmentId = Guid.NewGuid();
        var request = new DeleteAppointmentRequest (nonExistentAppointmentId);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AppointmentId)
              .WithErrorMessage("The specified appointment does not exist.");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Appointment_Exists()
    {
        // Arrange
        var existingAppointmentId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
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
        _dbFactory.AddData(new List<Appointment>()
        {
            new Appointment
            {
                Id = existingAppointmentId,
                DoctorId = doctorId,
                StartDate = new DateTime(2025, 5, 5, 10, 0, 0),
                EndDate = new DateTime(2025, 5, 5, 11, 0, 0),
                ClinicId = TestHelper.ClinicId,
                Name = "Test",
                Description = "Test",
                ShiftId = shiftId,
            }
        });

        var request = new DeleteAppointmentRequest(existingAppointmentId );

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AppointmentId);
    }
}
