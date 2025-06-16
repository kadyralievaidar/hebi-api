using FluentValidation.TestHelper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.RequestHandling.Validators;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.Users.Validators;

[TestFixture]
public class CreateUserRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private CreateUserRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        _validator = new CreateUserRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_UserName_Already_Exists()
    {
        // Arrange
        var existingUserName = "ExistingUser";

        _dbFactory.AddData(new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = existingUserName,
                Email = "test@mail.com",
                ClinicId = TestHelper.ClinicId
            }
        });

        var dto = new RegisterUserDto
        {
            UserName = existingUserName,
            Password = "Test*123",
            ConfirmPassword = "Test*123",
            Email = "newemail@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "1234567890"
        };

        var request = new CreateUserRequest(new CreateUserDto(dto, TestHelper.ClinicId));

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.RegisterDto.UserName)
              .WithErrorMessage("The user with this username already exists");
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Already_Exists()
    {
        // Arrange
        var existingEmail = "existingemail@mail.com";

        _dbFactory.AddData(new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "AnotherUser",
                Email = existingEmail,
                ClinicId = TestHelper.ClinicId
            }
        });

        var dto = new RegisterUserDto
        {
            UserName = "NewUser",
            Password = "Test*123",
            ConfirmPassword = "Test*123",
            Email = existingEmail,
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "1234567890"
        };

        var request = new CreateUserRequest(new CreateUserDto(dto, TestHelper.ClinicId));

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.RegisterDto.Email)
              .WithErrorMessage("The user with this email already exists");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_All_Valid()
    {
        // Arrange
        var validClinicId = TestHelper.ClinicId;
        _dbFactory.AddData(new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "ExistingUser",
                Email = "existing@mail.com",
                ClinicId = validClinicId
            }
        });

        var dto = new RegisterUserDto
        {
            UserName = "UniqueUser",
            Password = "Test*123",
            ConfirmPassword = "Test*123",
            Email = "unique@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "1234567890"
        };

        var request = new CreateUserRequest(new CreateUserDto(dto, validClinicId));

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
