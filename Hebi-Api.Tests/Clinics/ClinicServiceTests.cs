using FluentAssertions;
using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Hebi_Api.Tests.Clinics;

[TestFixture]
public class ClinicServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IClinicsService _clinicsService;
    private HebiDbContext _dbContext;

    private UserManager<ApplicationUser> _userManager;


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

        _clinicsService = new ClinicsService(_unitOfWorkSqlite, TestHelper.CreateHttpContext().Object);
    }

    [Test]
    public async Task CreateClinic_ShouldWorks_Properly()
    {
        //Assert
        var dto = new CreateClinicDto()
        {
            Name = "Test",
            Email = "test@test.test",
            PhoneNumber = "1234567890"
        };

        //Act
        var result = await _clinicsService.CreateClinicAsync(dto);

        //Assert
        var clinic = await _unitOfWorkSqlite.ClinicRepository.GetByIdAsync(result);
        clinic.Should().NotBeNull();
        clinic.Name.Should().Be(dto.Name);
        clinic.Email.Should().Be(dto.Email);
        clinic.PhoneNumber.Should().Be(dto.PhoneNumber);
    }

    [Test]
    public async Task CreateClinic_WithUsersShouldWorks_Properly()
    {
        //Assert
        var doctor = new ApplicationUser()
        {
            UserName = "Test",
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            NormalizedUserName = "Test",
        };
        var doctor2 = new ApplicationUser()
        {
            UserName = "Test",
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            NormalizedUserName = "Test",
        };
        var doctor3 = new ApplicationUser()
        {
            UserName = "Test",
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            NormalizedUserName = "Test",
        };
        await _userManager.CreateAsync(doctor);
        await _userManager.CreateAsync(doctor2);
        await _userManager.CreateAsync(doctor3);

        var dto = new CreateClinicDto()
        {
            Name = "Test",
            Email = "test@test.test",
            PhoneNumber = "1234567890",
            DoctorIds = [doctor.Id, doctor2.Id, doctor3.Id],
        };
        //Act
        var result = await _clinicsService.CreateClinicAsync(dto);

        //Assert
        var clinic = await _unitOfWorkSqlite.ClinicRepository.GetByIdAsync(result);
        clinic.Should().NotBeNull();
        clinic.Name.Should().Be(dto.Name);
        clinic.Email.Should().Be(dto.Email);
        clinic.PhoneNumber.Should().Be(dto.PhoneNumber);
        var users = await _unitOfWorkSqlite.UsersRepository.WhereAsync(x => dto.DoctorIds.Contains(x.Id));

        foreach (var user in users)
        {
            user.ClinicId.Should().Be(result);
        }
    }

    [Test]
    public async Task UpdateClinic_WithDoctors_ShouldWorkProper()
    {
        //Assert
        var doctor = new ApplicationUser()
        {
            UserName = "Test",
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            NormalizedUserName = "Test",
        };
        var doctor2 = new ApplicationUser()
        {
            UserName = "Test",
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            NormalizedUserName = "Test",
        };
        var doctor3 = new ApplicationUser()
        {
            UserName = "Test",
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            NormalizedUserName = "Test",
        };
        await _userManager.CreateAsync(doctor);
        await _userManager.CreateAsync(doctor2);
        await _userManager.CreateAsync(doctor3);

        var clinicId = Guid.NewGuid();
        _dbFactory.AddData(new List<Clinic>()
        {
            new()
            {
                Id = clinicId,
                Name = "Test",
                PhoneNumber = "0779898989",
                Email = "TestMail",
                Location = "TestLocation"
            }
        });
        var dto = new CreateClinicDto()
        {
            Name = "Test",
            Email = "test@test.test",
            PhoneNumber = "1234567890",
            DoctorIds = [doctor.Id, doctor2.Id, doctor3.Id],
        };
        //Act
        var result = await _clinicsService.UpdateClinicAsync(clinicId, dto);

        //Assert
        var clinic = await _unitOfWorkSqlite.ClinicRepository.GetByIdAsync(clinicId);
        clinic.Should().NotBeNull();
        clinic.Name.Should().Be(dto.Name);
        clinic.Email.Should().Be(dto.Email);
        clinic.PhoneNumber.Should().Be(dto.PhoneNumber);
        var users = await _unitOfWorkSqlite.UsersRepository.WhereAsync(x => dto.DoctorIds.Contains(x.Id));
        foreach (var user in users)
        {
            user.ClinicId.Should().Be(clinicId);
        }
    }
}
