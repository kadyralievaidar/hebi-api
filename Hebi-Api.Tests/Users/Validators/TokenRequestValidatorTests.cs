using FluentValidation.TestHelper;
using Hebi_Api.Features.Appointments.RequestHandling.Validators;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.RequestHandling.Validators;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OpenIddict.Abstractions;

namespace Hebi_Api.Tests.Users.Validators;

[TestFixture]
public class TokenRequestValidatorTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private TokenRequestValidator _validator;
    private UserManager<ApplicationUser> _userManager;
    private HebiDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbFactory = new UnitOfWorkFactory();
        _dbContext = _dbFactory.GetDbContext();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddScoped(typeof(RoleManager<>));
        serviceCollection.AddScoped(_ => _dbContext);
        serviceCollection.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                        .AddUserManager<UserManager<ApplicationUser>>()
                        .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                         .AddEntityFrameworkStores<HebiDbContext>()
                        .AddDefaultTokenProviders();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        _validator = new TokenRequestValidator(_unitOfWorkSqlite, _userManager);
    }
    [Test]
    public async Task Should_Have_Error_When_Username_Is_Null()
    {
        // Arrange
        var request = new TokenRequest(new OpenIddictRequest() { Username = null});

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Request.Username)
              .WithErrorMessage("User name can't be null");
    }

    [Test]
    public async Task Should_Have_Error_When_Username_Is_Empty()
    {
        var request = new TokenRequest(new OpenIddictRequest { Username = "", Password = "whatever" });

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Request.Username)
              .WithErrorMessage("User name can't be null");
    }

    [Test]
    public async Task Should_Have_Error_When_Username_Exists()
    {
        // Arrange
        var username = "existinguser";
        var normalizedUsername = username.ToUpperInvariant();

        _dbFactory.AddData(new List<ApplicationUser>
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = username,
                NormalizedUserName = normalizedUsername
            }
        });

        var request = new TokenRequest(new OpenIddictRequest() { Username = username});

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Request.Username);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Username_Does_Not_Exist()
    {
        // Arrange
        var request = new TokenRequest(new OpenIddictRequest() { Username = "newuser"});

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Request.Username)
              .WithErrorMessage("User not found");
    }
}
