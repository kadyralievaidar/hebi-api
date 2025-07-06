using FluentAssertions;
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
public class ChangeUserInfoRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private ChangeUserInfoRequestValidator _validator;
    private Guid _existingUserId;

    [SetUp]
    public void SetUp()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _existingUserId = Guid.NewGuid();
        _dbFactory.AddData(new List<ApplicationUser>
        {
            new ()
            {
                Id = _existingUserId,
                UserName = "testuser",
                Email = "test@example.com"
            }
        });
        _validator = new ChangeUserInfoRequestValidator(_unitOfWorkSqlite);
    }

    [Test]
    public async Task Should_Pass_Validation_When_User_Exists()
    {
        var dto = new BasicInfoDto()
        {
            UserId = _existingUserId
        };
        var request = new ChangeUserInfoRequest(dto);

        var result = await _validator.TestValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task Should_Fail_Validation_When_User_Does_Not_Exist()
    {
        var dto = new BasicInfoDto()
        {
            UserId = Guid.NewGuid()
        };
        var request = new ChangeUserInfoRequest(dto);

        var result = await _validator.TestValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Dto.UserId)
              .WithErrorMessage("User not found");
    }
}
