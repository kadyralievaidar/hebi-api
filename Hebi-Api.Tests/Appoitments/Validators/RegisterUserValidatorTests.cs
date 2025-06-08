using Hebi_Api.Features.Users.RequestHandling.Validators;
using FluentValidation.TestHelper;
using Hebi_Api.Features.Users.Dtos;
using NUnit.Framework;

namespace Hebi_Api.Tests.Appoitments.Validators;

[TestFixture]
public class RegisterUserValidatorTests
{
    private RegisterUserValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new RegisterUserValidator();
    }

    [Test]
    public void Should_Have_Error_When_UserName_Is_Empty()
    {
        // Arrange
        var user = new RegisterUserDto {
            UserName = "",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("UserName is required");
    }

    [Test]
    public void Should_Have_Error_When_UserName_Is_Short()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "ky",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("UserName must be at least 4 characters");
    }

    [Test]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "",
            ConfirmPassword = "",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required");
    }

    [Test]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "Aa1*",
            ConfirmPassword = "Aa1*",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 6 characters");
    }

    [Test]
    public void Should_Have_Error_When_Password_Has_No_Uppercase()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "test*123",
            ConfirmPassword = "test*123",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter");
    }

    [Test]
    public void Should_Have_Error_When_Password_Has_No_Lowercase()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TEST*123",
            ConfirmPassword = "TEST*123",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter");
    }

    [Test]
    public void Should_Have_Error_When_Password_Has_No_Number()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "Test*Test",
            ConfirmPassword = "Test*Test",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one number");
    }

    [Test]
    public void Should_Have_Error_When_Password_Has_No_Special_Character()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "Test1234",
            ConfirmPassword = "Test1234",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one special character");
    }

    [Test]
    public void Should_Have_Error_When_ConfirmPassword_Is_Empty()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
            .WithErrorMessage("ConfirmPassword is required");
    }

    [Test]
    public void Should_Have_Error_When_ConfirmPassword_Is_Not_Valid()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
            .WithErrorMessage("Passwords do not match");
    }

    [Test]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Test]
    public void Should_Have_Error_When_Email_Is_Not_Valid()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "345QWE",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email address");
    }

    [Test]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name is required");
    }

    [Test]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = ""
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name is required");
    }

    [Test]
    public void Should_Not_Have_Error_When_User_Is_Valid()
    {
        // Arrange
        var user = new RegisterUserDto
        {
            UserName = "Test1",
            Password = "TesT*00",
            ConfirmPassword = "TesT*00",
            Email = "Test@mail.com",
            FirstName = "Test",
            LastName = "Test"
        };
        // Act
        var result = _validator.TestValidate(user);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}
