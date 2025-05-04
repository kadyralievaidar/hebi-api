using FluentValidation.TestHelper;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.RequestHandling.Validators;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.Appoitments.Validators;

[TestFixture]
public class CreateAppointmentValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private CreateAppointmentValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        _validator = new CreateAppointmentValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_Appointment_Conflicts()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var clinicId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();
        var request = new CreateAppointmentRequest(new CreateAppointmentDto()
        {
            DoctorId = doctorId,
            StartDateTime = new DateTime(2025, 5, 4, 10, 0, 0),
            EndDateTime = new DateTime(2025, 5, 4, 11, 0, 0),
        });

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
        _dbFactory.AddData(new List<Appointment>() { new()
        {
            DoctorId = doctorId,
            Name = "Test",
            Description = "Test",
            StartDate = new DateTime(2025, 5, 4, 10, 0, 0),
            EndDate = new DateTime(2025, 5, 4, 11, 0, 0),
            PatientId = null,
            ShiftId = null,
            ClinicId = TestHelper.ClinicId
        } });

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_No_Appointment_Conflict()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var request = new CreateAppointmentRequest(new CreateAppointmentDto()
        {
                DoctorId = doctorId,
                StartDateTime = new DateTime(2025, 5, 4, 14, 0, 0),
                EndDateTime = new DateTime(2025, 5, 4, 15, 0, 0)

        });

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
}
