using FluentAssertions;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Hebi_Api.Tests.Appoitments;

[TestFixture]

public class AppointmentServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IAppointmentsService _appointmentService;
    private HebiDbContext _dbContext;
    private readonly Guid _clinicId = TestHelper.ClinicId;

    private UserManager<ApplicationUser> _userManager;

    private Mock<RoleManager<IdentityRole<Guid>>> _roleLogger;

    [SetUp]
    public void Setup()
    {
        _dbFactory = new UnitOfWorkFactory();
        _dbContext = _dbFactory.GetDbContext();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddScoped(typeof(RoleManager<>));
        _roleLogger = new Mock<RoleManager<IdentityRole<Guid>>>();

        serviceCollection.AddScoped(_ => _roleLogger.Object);
        serviceCollection.AddScoped(_ => _dbContext);

        serviceCollection.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddEntityFrameworkStores<HebiDbContext>()
            .AddDefaultTokenProviders();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        _appointmentService = new AppointmentsService(_unitOfWorkSqlite, TestHelper.CreateHttpContext().Object);

    }

    [Test]
    public async Task CreateAppointment_Should_Work_Proper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();
        var clinic = new Clinic()
        {
            Id = _clinicId,
            Name = "TestClinic",
            CreatedBy = Guid.NewGuid()
        };
        _dbFactory.AddData(new List<Clinic>()
        {
            clinic
        });
        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = _clinicId,
            Clinic = clinic,
            NormalizedUserName = "TESTPATIENT"
        };
        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = _clinicId,
            Clinic = clinic,
            NormalizedUserName = "TESTDOCTOR"
        };

        await _userManager.CreateAsync(doctor);
        await _userManager.CreateAsync(patient);

        _dbFactory.AddData(new List<Shift>() 
        { 
            new() 
            {
                Id = shiftId,
                EndTime = DateTime.UtcNow.AddHours(4),
                StartTime = DateTime.UtcNow.AddHours(-4),
                ClinicId = _clinicId,
                DoctorId = doctorId
            } 
        });

        var dto = new CreateAppointmentDto()
        {
            DoctorId = doctorId,
            EndDateTime = DateTime.Now.AddMinutes(30),
            StartDateTime = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription"
        };
        //Act
        var result = await _appointmentService.CreateAppointment(dto);

        //Assert 
        var appointment = await _unitOfWorkSqlite.AppointmentRepository.GetByIdAsync(result);
        appointment.Should().NotBeNull();
        appointment.ShiftId.Should().Be(shiftId);
        appointment.PatientId.Should().Be(patientId);
        appointment.DoctorId.Should().Be(doctorId);
        appointment.EndDate.Should().Be(dto.EndDateTime);
        appointment.StartDate.Should().Be(dto.StartDateTime);
    }

    [Test]
    public async Task DeleteAppointment_ShouldWorksProper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();
        var clinic = new Clinic()
        {
            Id = _clinicId,
            Name = "TestClinic",
            CreatedBy = Guid.NewGuid()
        };
        _dbFactory.AddData(new List<Clinic>()
        {
            clinic
        });
        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = _clinicId,
            Clinic = clinic,
            NormalizedUserName = "TESTPATIENT"
        };
        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = _clinicId,
            Clinic = clinic,
            NormalizedUserName = "TESTDOCTOR"
        };

        await _userManager.CreateAsync(doctor);
        await _userManager.CreateAsync(patient);

        _dbFactory.AddData(new List<Shift>()
        {
            new()
            {
                Id = shiftId,
                EndTime = DateTime.UtcNow.AddHours(4),
                StartTime = DateTime.UtcNow.AddHours(-4),
                ClinicId = _clinicId,
                DoctorId = doctorId
            }
        });

        var appointmentId = Guid.NewGuid();
        var dto = new Appointment()
        {
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription"
        };
        //Act
        await _appointmentService.DeleteAppointment(dto.Id);

        //Assert 
        var appointment = await _unitOfWorkSqlite.AppointmentRepository.GetByIdAsync(result);
        appointment.Should().BeNull();
    }
}
