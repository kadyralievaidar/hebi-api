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
public class UpdateAppointmentValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private UpdateAppointmentRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        _validator = new UpdateAppointmentRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_Appointment_Does_Not_Exist()
    {
        // Arrange
        var request = new UpdateAppointmentRequest(Guid.NewGuid(), new UpdateAppointmentDto()
        {

            DoctorId = Guid.NewGuid(),
            StartDateTime = DateTime.UtcNow,
            EndDateTime = DateTime.UtcNow.AddHours(1)
        });

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AppointmentId);
    }

    [Test]
    public async Task Should_Have_Error_When_Appointment_Time_Conflicts()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();
        var appointmentId = Guid.NewGuid();

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
            new()
            {
                Id = appointmentId,
                DoctorId = doctorId,
                StartDate = new DateTime(2025, 5, 4, 12, 0, 0),
                EndDate = new DateTime(2025, 5, 4, 13, 0, 0),
                Name = "Test",
                Description = "Test",   
                ClinicId = TestHelper.ClinicId,
                ShiftId = shiftId
            },
            new()
            {
                Id = Guid.NewGuid(), // Conflicting appointment
                DoctorId = doctorId,
                StartDate = new DateTime(2025, 5, 4, 12, 30, 0),
                EndDate = new DateTime(2025, 5, 4, 13, 30, 0),
                Name = "Test",
                Description = "Test",
                ClinicId = TestHelper.ClinicId,
                ShiftId = shiftId
            }
        });

        var request = new UpdateAppointmentRequest(appointmentId, new UpdateAppointmentDto()
        {
            DoctorId = doctorId,
            StartDateTime = new DateTime(2025, 5, 4, 12, 30, 0),
            EndDateTime = new DateTime(2025, 5, 4, 13, 30, 0)
        });

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Valid()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();
        var appointmentId = Guid.NewGuid();
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
            new()
            {
                Id = appointmentId,
                DoctorId = doctorId,
                StartDate = new DateTime(2025, 5, 4, 8, 0, 0),
                EndDate = new DateTime(2025, 5, 4, 9, 0, 0),
                ClinicId = TestHelper.ClinicId,
                Name = "Test",
                Description = "Test",
                ShiftId = shiftId
            }
        });

        var request = new UpdateAppointmentRequest(appointmentId, new UpdateAppointmentDto()
        {
                DoctorId = doctorId,
                StartDateTime = new DateTime(2025, 5, 4, 10, 0, 0),
                EndDateTime = new DateTime(2025, 5, 4, 11, 0, 0)
        });

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AppointmentId);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
}
