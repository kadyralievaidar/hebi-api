using FluentValidation.TestHelper;
using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.RequestHandling.Validators;
using Moq;
using NUnit.Framework;

namespace Hebi_Api.Tests.Users.Validators;

[TestFixture]
public class ChangeUserRoleValidatorTests
{
    private Mock<IUsersRepository> _usersRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private ChangeUserRoleValidator _validator;

    [SetUp]
    public void Setup()
    {
        _usersRepositoryMock = new Mock<IUsersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(x => x.UsersRepository)
            .Returns(_usersRepositoryMock.Object);

        _validator = new ChangeUserRoleValidator(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Should_Pass_When_UserId_Is_Null()
    {
        var model = new ChangeUserRoleRequest(null, string.Empty);

        var result = await _validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task Should_Pass_When_User_Exists()
    {
        var userId = Guid.NewGuid();
        _usersRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(new ApplicationUser { Id = userId });

        var model = new ChangeUserRoleRequest(userId, string.Empty);

        var result = await _validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task Should_Fail_When_User_Not_Found()
    {
        var userId = Guid.NewGuid();
        _usersRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((ApplicationUser)null!);

        var model = new ChangeUserRoleRequest(userId, string.Empty);

        var result = await _validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User with the specified ID does not exist.");
    }

    [TestCase(Consts.SuperAdmin)]
    [TestCase(Consts.Admin)]
    [TestCase(Consts.Doctor)]
    [TestCase(Consts.Patient)]
    [TestCase(Consts.Individual)]
    public async Task Should_Pass_For_Valid_RoleName(string roleName)
    {
        var model = new ChangeUserRoleRequest(null, roleName);

        var result = await _validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.RoleName);
    }

    [Test]
    public async Task Should_Fail_For_Invalid_RoleName()
    {
        var model = new ChangeUserRoleRequest(null,"Hacker");

        var result = await _validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.RoleName)
            .WithErrorMessage("Invalid role name");
    }
}
