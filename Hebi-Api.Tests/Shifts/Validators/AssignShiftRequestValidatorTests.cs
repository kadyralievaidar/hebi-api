using FluentValidation.TestHelper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using Hebi_Api.Features.Shifts.RequestHandling.Validators;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.Shifts.Validators;

[TestFixture]
public class AssignShiftRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private AssignShiftRequestValidator _validator;
    private Guid _doctorId;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        _doctorId = TestHelper.UserId;

        _validator = new AssignShiftRequestValidator(
            _unitOfWorkSqlite,
            TestHelper.CreateHttpContext(_doctorId).Object);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_No_Overlap()
    {
        // Arrange
        var shift1 = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2025, 09, 10, 8, 0, 0),
            EndTime = new DateTime(2025, 09, 10, 12, 0, 0),
            DoctorId = _doctorId,
            ClinicId = TestHelper.ClinicId
        };

        var newShift = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2025, 09, 10, 13, 0, 0),
            EndTime = new DateTime(2025, 09, 10, 17, 0, 0),
            ClinicId = TestHelper.ClinicId
        };

        _dbFactory.AddData(new List<Shift> { shift1, newShift });

        var request = new AssignShiftRequest(_doctorId, newShift.Id);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Should_Have_Error_When_Shifts_Overlap()
    {
        // Arrange
        var shift1 = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2025, 09, 10, 8, 0, 0),
            EndTime = new DateTime(2025, 09, 10, 12, 0, 0),
            DoctorId = _doctorId,
            ClinicId = TestHelper.ClinicId
        };

        var newShift = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2025, 09, 10, 11, 0, 0),
            EndTime = new DateTime(2025, 09, 10, 15, 0, 0),
            ClinicId = TestHelper.ClinicId
        };

        _dbFactory.AddData(new List<Shift> { shift1, newShift });

        var request = new AssignShiftRequest(_doctorId, newShift.Id);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r)
              .WithErrorMessage("Doctor can not be assigned.");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Shifts_Touch_But_Not_Overlap()
    {
        // Arrange
        var shift1 = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2025, 09, 10, 8, 0, 0),
            EndTime = new DateTime(2025, 09, 10, 12, 0, 0),
            DoctorId = _doctorId,
            ClinicId = TestHelper.ClinicId
        };

        var newShift = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2025, 09, 10, 12, 0, 0),
            EndTime = new DateTime(2025, 09, 10, 16, 0, 0),
            ClinicId = TestHelper.ClinicId
        };

        _dbFactory.AddData(new List<Shift> { shift1, newShift });

        var request = new AssignShiftRequest(_doctorId, newShift.Id);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Should_Have_Error_When_Shift_Not_Found()
    {
        // Arrange
        var request = new AssignShiftRequest(_doctorId, Guid.NewGuid());

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r)
              .WithErrorMessage("Doctor can not be assigned.");
    }
}
