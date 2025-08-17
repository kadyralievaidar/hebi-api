using FluentValidation.TestHelper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Validators;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.ShiftTemplates.Validators;
public class GetShiftTemplateRequestValidatoTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private GetShiftTemplateByIdRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        _validator = new GetShiftTemplateByIdRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_ShiftTemplate_Not_Exist()
    {
        // Arrange
        var request = new GetShiftTemplateByIdRequest(Guid.NewGuid());

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

        var request = new GetShiftTemplateByIdRequest(template.Id);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.ShiftTemplateId);
    }
}
