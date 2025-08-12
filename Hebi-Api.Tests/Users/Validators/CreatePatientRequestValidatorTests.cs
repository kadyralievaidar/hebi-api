using FluentValidation.TestHelper;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.RequestHandling.Validators;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;

namespace Hebi_Api.Tests.Users.Validators;

[TestFixture]
public class CreatePatientRequestValidatorTests
{
    private Mock<IUsersRepository> _usersRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IStringLocalizer> _stringLocalizerMock;
    private CreatePatientRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _usersRepositoryMock = new Mock<IUsersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _stringLocalizerMock = new Mock<IStringLocalizer>();

        _unitOfWorkMock.Setup(x => x.UsersRepository)
            .Returns(_usersRepositoryMock.Object);

        _validator = new CreatePatientRequestValidator(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Should_Fail_When_User_With_Phone_Exists()
    {
        var phone = "+123456789";
        _usersRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(u => u.PhoneNumber == phone, default))
            .ReturnsAsync(new ApplicationUser { PhoneNumber = phone });

        var model = new CreatePatientRequest(new CreatePatientDto { PhoneNumber = phone, FirstName = "Test", LastName = "Test" });

        var result = await _validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Dto.PhoneNumber);
    }

    [Test]
    public async Task Should_Pass_When_User_With_Phone_Not_Exists()
    {
        var phone = "+123456789";
        _usersRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(u => u.PhoneNumber == phone, default))
            .ReturnsAsync((ApplicationUser)null!);

        var model = new CreatePatientRequest(new CreatePatientDto { PhoneNumber = phone, FirstName = "Test", LastName = "Test" });

        var result = await _validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Dto.PhoneNumber);
    }

    [Test]
    public async Task Should_Fail_When_PhoneNumber_Is_Empty()
    {
        var model = new CreatePatientRequest(new CreatePatientDto { PhoneNumber = string.Empty, FirstName = "Test", LastName = "Test" });

        var result = await _validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Dto.PhoneNumber);
    }
}
