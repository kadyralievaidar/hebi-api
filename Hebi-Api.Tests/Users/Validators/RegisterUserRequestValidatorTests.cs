using Hebi_Api.Features.Users.RequestHandling.Validators;
using FluentValidation.TestHelper;
using Hebi_Api.Features.Users.Dtos;
using NUnit.Framework;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;

namespace Hebi_Api.Tests.Users.Validators;

[TestFixture]
public class RegisterUserRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private RegisterUserRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _validator = new RegisterUserRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Have_Error_When_UserName_Is_Empty()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"
        };

        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.UserName)
              .WithErrorMessage("UserName is required");
    }

    [Test]
    public async Task Should_Have_Error_When_UserName_Is_Short()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "ky",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"
        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.UserName)
            .WithErrorMessage("UserName must be at least 4 characters");
    }

    [Test]
    public async Task Should_Have_Error_When_Password_Is_Empty()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "",
            ConfirmPassword = "",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"
        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Password)
            .WithErrorMessage("Password is required");
    }

    [Test]
    public async Task Should_Have_Error_When_Password_Is_Too_Short()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "Aa1*",
            ConfirmPassword = "Aa1*",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Password)
            .WithErrorMessage("Password must be at least 6 characters");
    }

    [Test]
    public async Task Should_Have_Error_When_Password_Has_No_Uppercase()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "test*123",
            ConfirmPassword = "test*123",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter");
    }

    [Test]
    public async Task Should_Have_Error_When_Password_Has_No_Lowercase()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TEST*123",
            ConfirmPassword = "TEST*123",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter");
    }

    [Test]
    public async Task Should_Have_Error_When_Password_Has_No_Number()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "Test*Test",
            ConfirmPassword = "Test*Test",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Password)
            .WithErrorMessage("Password must contain at least one number");
    }

    [Test]
    public async Task Should_Have_Error_When_Password_Has_No_Special_Character()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "Test1234",
            ConfirmPassword = "Test1234",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Password)
            .WithErrorMessage("Password must contain at least one special character");
    }

    [Test]
    public async Task Should_Have_Error_When_ConfirmPassword_Is_Empty()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.ConfirmPassword)
            .WithErrorMessage("ConfirmPassword is required");
    }

    [Test]
    public async Task Should_Have_Error_When_ConfirmPassword_Is_Not_Valid()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.ConfirmPassword)
            .WithErrorMessage("Passwords do not match");
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_EmptyAsync()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Email)
            .WithErrorMessage("Email is required");
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_Not_Valid()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "345QWE",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.Email)
            .WithErrorMessage("Invalid email address");
    }

    [Test]
    public async Task Should_Have_Error_When_FirstName_Is_Empty()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "",
            LastName = "Test",
            PhoneNumber = "955587878895"
        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.FirstName)
            .WithErrorMessage("First name is required");
    }

    [Test]
    public async Task Should_Have_Error_When_LastName_Is_Empty()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "",
            PhoneNumber = "955587878895"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.LastName)
            .WithErrorMessage("Last name is required");
    }

    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Is_Empty()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = ""

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.PhoneNumber)
            .WithErrorMessage("Phone number is required");
    }


    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Is_Not_Valid()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "Test78"

        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RegisterUserDto.PhoneNumber)
            .WithErrorMessage("Phone number must be valid and contain 7–12 digits");
    }

    [Test]
    public async Task Should_Not_Have_Error_When_User_Is_Valid()
    {
        // Arrange
        var dto = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "955587878898"
        };
        // Act
        var result = await _validator.TestValidateAsync(new RegisterUserRequest(dto));
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}