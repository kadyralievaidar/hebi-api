using FluentAssertions;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.UserCards.Services;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.Services;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace Hebi_Api.Tests.Users;

[TestFixture]
public class UsersServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IUsersService _usersService;
    private HebiDbContext _dbContext;
    private UserManager<ApplicationUser> _userManager;
    private RoleManager<IdentityRole<Guid>> _roleManager;
    private Guid DefaultClinicId = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        _dbFactory = new UnitOfWorkFactory();
        _dbContext = _dbFactory.GetDbContext();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging();
        serviceCollection.AddScoped(_ => _dbContext);
        var claims = new List<Claim>
        {
            new Claim(Consts.UserId, TestHelper.UserId.ToString())
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        contextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);
        serviceCollection.AddSingleton(contextAccessorMock.Object);
        // Add Identity services
        serviceCollection.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddEntityFrameworkStores<HebiDbContext>()
            .AddDefaultTokenProviders();

        serviceCollection.AddSingleton(Mock.Of<IOpenIddictApplicationManager>());
        serviceCollection.AddSingleton(_unitOfWorkSqlite);
        var clinicServiceMock = new Mock<IClinicsService>();
        clinicServiceMock
            .Setup(x => x.CreateDefaultClinic())
            .ReturnsAsync(DefaultClinicId);
        serviceCollection.AddSingleton(clinicServiceMock.Object);
        serviceCollection.AddSingleton(Mock.Of<IUserCardsService>());

        // Add user principal factory
        serviceCollection.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<Guid>>>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        // Seed a test user
        var testUser = new ApplicationUser()
        {
            Id = TestHelper.UserId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        _dbFactory.AddData(new List<ApplicationUser>
        {
            testUser
        });
        _dbFactory.DetachForReload(testUser);
        // Now initialize UsersService with all required dependencies (use mocks or actual objects as needed)

        _usersService = new UsersService(serviceProvider);
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
    public async Task ChangeUserRole()
    {

        await _roleManager.CreateAsync(new IdentityRole<Guid>("OldTest"));
        await _roleManager.CreateAsync(new IdentityRole<Guid>("Test"));
        var user = new ApplicationUser()
        {
            UserName = "TestPatient2",
            FirstName = "Patient2",
            LastName = "PatientLastName2",
            Email = "test@test.com2",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT2",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        _dbFactory.AddData(new List<ApplicationUser>() { user});
        await _userManager.AddToRoleAsync(user, "OldTest");

        await _usersService.ChangeUserRole(user.Id, "Test");

        var userDb = await _unitOfWorkSqlite.UsersRepository.FirstOrDefaultAsync(x => x.Id == user.Id);
        var role = await _userManager.IsInRoleAsync(userDb, "Test");

        role.Should().BeTrue();
    }

    [Test]
    public async Task ChangeUserRole_WhenIdIsNull()
    {
        await _roleManager.CreateAsync(new IdentityRole<Guid>("OldTest"));
        await _roleManager.CreateAsync(new IdentityRole<Guid>("Test"));
        await _usersService.ChangeUserRole(null, "Test");
        var user = await _unitOfWorkSqlite.UsersRepository.FirstOrDefaultAsync(x => x.Id == TestHelper.UserId);

        await _userManager.AddToRoleAsync(user, "OldTest");

        var userDb = await _unitOfWorkSqlite.UsersRepository.FirstOrDefaultAsync(x => x.Id == TestHelper.UserId);
        var role = await _userManager.IsInRoleAsync(userDb, "Test");

        role.Should().BeTrue();
    }

    [Test]
    public async Task ChangeUserRole_WhenUserIsInvidiviudal()
    {
        var defaultClinic = new Clinic
        {
            Id = DefaultClinicId,
            Name = "DEFAULT"
        };
        _dbFactory.AddData(new List<Clinic> { defaultClinic });
        await _roleManager.CreateAsync(new IdentityRole<Guid>("OldTest"));
        await _roleManager.CreateAsync(new IdentityRole<Guid>(Consts.Individual));
        var user = await _unitOfWorkSqlite.UsersRepository.FirstOrDefaultAsync(x => x.Id == TestHelper.UserId);

        await _userManager.AddToRoleAsync(user, "OldTest");

        await _usersService.ChangeUserRole(null, Consts.Individual);
        var userDb = await _unitOfWorkSqlite.UsersRepository.FirstOrDefaultAsync(x => x.Id == TestHelper.UserId);
        var role = await _userManager.IsInRoleAsync(userDb, Consts.Individual);

        var defaultClinicDb = await _unitOfWorkSqlite.ClinicRepository.GetClinicById(userDb.ClinicId.Value);
        role.Should().BeTrue();
        defaultClinic.Name.Should().Be("DEFAULT");
    }
}
