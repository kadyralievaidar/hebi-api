using FluentValidation.TestHelper;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.RequestHandling.Validators;
using NUnit.Framework;

namespace Hebi_Api.Tests.Appoitments.Validators;

[TestFixture]
public class GetPagedListOfAppointmentRequestValidatorTests
{
    private GetPagedListOfAppointmentRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetPagedListOfAppointmentRequestValidator();
    }

    [Test]
    public async Task Should_Have_Error_When_StartDate_Greater_Than_EndDate()
    {
        // Arrange
        var request = new GetPagedListOfAppointmentRequest(new GetPagedListOfAppointmentDto
            {
                StartDate = new DateTime(2025, 5, 5, 12, 0, 0),
                EndDate = new DateTime(2025, 5, 5, 10, 0, 0)
            }
        );

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Dto.EndDate)
              .WithErrorMessage("Start date time can't be greater than End date time");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_StartDate_Less_Than_Or_Equal_To_EndDate()
    {
        // Arrange
        var request = new GetPagedListOfAppointmentRequest(new GetPagedListOfAppointmentDto
            {
                StartDate = new DateTime(2025, 5, 5, 9, 0, 0),
                EndDate = new DateTime(2025, 5, 5, 10, 0, 0)
            }
        );

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Dto.StartDate);
    }
}
