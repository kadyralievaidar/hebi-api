using FluentAssertions;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.Services;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OpenIddict.Abstractions;

namespace Hebi_Api.Tests.Users;

[TestFixture]
public class UsersServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IUsersService _usersService;
    private HebiDbContext _dbContext;

    private UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;
    private RoleManager<IdentityRole<Guid>> _roleManager;
    [SetUp]
    public void Setup()
    {
        _dbFactory = new UnitOfWorkFactory();
        _dbContext = _dbFactory.GetDbContext();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging();
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.AddScoped(_ => _dbContext);

        // Add Identity services
        serviceCollection.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddEntityFrameworkStores<HebiDbContext>()
            .AddDefaultTokenProviders();

        // Add user principal factory
        serviceCollection.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<Guid>>>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        _signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

        // Seed a test user
        _dbFactory.AddData(new List<ApplicationUser>
        {
            new()
            {
                Id = TestHelper.UserId,
                UserName = "TestPatient",
                FirstName = "Patient",
                LastName = "PatientLastName",
                Email = "test@test.com",
                ClinicId = TestHelper.ClinicId,
                NormalizedUserName = "TESTPATIENT"
            }
        });

        // Now initialize UsersService with all required dependencies (use mocks or actual objects as needed)
        _usersService = new UsersService(
            _userManager,
            _signInManager,
            Mock.Of<IOpenIddictApplicationManager>(),
            TestHelper.CreateHttpContext().Object,
            _unitOfWorkSqlite,
            Mock.Of<IClinicsService>()
        );
    }

    [Test]
    public async Task ChangeUserInfo_ShouldUpdateUserFields()
    {
        // Arrange
        var dto = new BasicInfoDto
        {
            UserId = TestHelper.UserId,
            FirstName = "UpdatedFirst",
            LastName = "UpdatedLast",
            Email = "updated@email.com",
            PhoneNumber = "+1234567890"
        };

        // Act
        await _usersService.ChangeBasicInfo(dto);

        // Assert
        var updatedUser = await _unitOfWorkSqlite.UsersRepository.GetByIdAsync(TestHelper.UserId);
        updatedUser.Should().NotBeNull();
        updatedUser.FirstName.Should().Be(dto.FirstName);
        updatedUser.LastName.Should().Be(dto.LastName);
        updatedUser.Email.Should().Be(dto.Email);
        updatedUser.PhoneNumber.Should().Be(dto.PhoneNumber);
    }

    [Test]
    public async Task ChangeUserInfo_ShouldThrow_IfUserNotFound()
    {
        // Arrange
        var dto = new BasicInfoDto
        {
            UserId = Guid.NewGuid(),
            FirstName = "Fake",
            LastName = "User",
            Email = "fake@email.com",
            PhoneNumber = "+0000000000"
        };

        // Act
        Func<Task> act = async () => await _usersService.ChangeBasicInfo(dto);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }
}
