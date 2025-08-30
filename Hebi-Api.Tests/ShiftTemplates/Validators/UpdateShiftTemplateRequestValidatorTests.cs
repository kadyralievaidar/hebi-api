using FluentValidation.TestHelper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.ShiftTemplates.Dtos;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Validators;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.ShiftTemplates.Validators;
public class UpdateShiftTemplateRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private UpdateShiftTemplateRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        _validator = new UpdateShiftTemplateRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_ShiftTemplate_Not_Exist()
    {
        // Arrange
        var request = new UpdateShiftTemplateRequest(Guid.NewGuid(), new CreateShiftTemplateDto());

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.ShiftTemplateId)
              .WithErrorMessage("Shift template with provided Id doesn't exist");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_ShiftTemplate_Exists()
    {
        // Arrange
        var template = new ShiftTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Night Shift",
            StartTime = new TimeOnly(22, 0),
            EndTime = new TimeOnly(6, 0),
            ClinicId = TestHelper.ClinicId
        };

        _dbFactory.AddData(new List<ShiftTemplate> { template });

        var request = new UpdateShiftTemplateRequest(template.Id, new CreateShiftTemplateDto());

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.ShiftTemplateId);
    }
    [Test]
    public async Task Should_Have_Error_When_Name_Already_Exists()
    {
        // Arrange
        var existingTemplate = new ShiftTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Night Shift",
            StartTime = new TimeOnly(22, 0),
            EndTime = new TimeOnly(6, 0),
            ClinicId = TestHelper.ClinicId
        };

        var existingTemplate2 = new ShiftTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Night Shift 2",
            StartTime = new TimeOnly(22, 0),
            EndTime = new TimeOnly(6, 0),
            ClinicId = TestHelper.ClinicId
        };

        _dbFactory.AddData(new List<ShiftTemplate> { existingTemplate, existingTemplate2 });

        var request = new UpdateShiftTemplateRequest(existingTemplate2.Id,new CreateShiftTemplateDto
        {
            Name = "Night Shift",
            StartTime = new TimeOnly(22, 0),
            EndTime = new TimeOnly(6, 0)
        });

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Dto.Name)
              .WithErrorMessage("Shift template with this name already exist");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Name_Is_Unique()
    {
        // Arrange
        var request = new UpdateShiftTemplateRequest(Guid.NewGuid(), new CreateShiftTemplateDto
        {
            Name = "Morning Shift",
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(18, 0)
        });

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Dto.Name);
    }
}
