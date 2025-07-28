using FluentAssertions;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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

        _appointmentService = new AppointmentsService(_unitOfWorkSqlite, TestHelper.CreateHttpContext().Object);
    }

    [Test]
    public async Task CreateAppointment_Should_Work_Proper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();

        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };
        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
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
                ClinicId = TestHelper.ClinicId,
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
        var result = await _appointmentService.CreateAppointmentAsync(dto);

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

        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };
        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
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
        var appointment = new Appointment()
        {
            Id = appointmentId,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };

        _dbFactory.AddData(new List<Appointment>(){ appointment });
        _dbFactory.DetachForReload(appointment);
        //Act
        await _appointmentService.DeleteAppointment(appointmentId);

        //Assert 
        var appointmentFromDb = await _unitOfWorkSqlite.AppointmentRepository.GetByIdAsync(appointmentId);
        appointmentFromDb.Should().BeNull();
    }

    [Test]
    public async Task UpdateAppointment_ShouldWorksProper()
    {
        #region entities
        //Arrange
        var doctorId = Guid.NewGuid();
        var newDoctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();

        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };

        var newPatientId = Guid.NewGuid();
        var newPatient = new ApplicationUser()
        {
            Id = newPatientId,
            UserName = "TestPatient2",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };
        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTDOCTOR"
        };
        var newDoctor = new ApplicationUser()
        {
            Id = newDoctorId,
            UserName = "TestDcotor2",
            FirstName = "DoctorName2",
            LastName = "DoctorLastName2",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTDOCTOR2"
        };

        await _userManager.CreateAsync(doctor);
        await _userManager.CreateAsync(newPatient);
        await _userManager.CreateAsync(patient);
        await _userManager.CreateAsync(newDoctor);

        var newShiftId = Guid.NewGuid();

        _dbFactory.AddData(new List<Shift>()
        {
            new()
            {
                Id = shiftId,
                EndTime = DateTime.UtcNow.AddHours(4),
                StartTime = DateTime.UtcNow.AddHours(-4),
                ClinicId = _clinicId,
                DoctorId = doctorId
            },
            new()
            {
                Id = newShiftId,
                EndTime= DateTime.UtcNow.AddHours(4),
                StartTime= DateTime.UtcNow.AddHours(-4),
                ClinicId = _clinicId,
                DoctorId = doctorId
            }
        });

        var appointmentId = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };

        _dbFactory.AddData([appointment]);
        _dbFactory.DetachForReload(appointment);

        var dto = new UpdateAppointmentDto()
        {
            DoctorId= newDoctorId,
            ShiftId= newShiftId,
            PatientId= newPatientId,
            Name= "NewName",
            Description= "NewDescription",
            StartDateTime = DateTime.Now,
            EndDateTime = DateTime.Now.AddMinutes(30)
        };
        #endregion
        //Act
        await _appointmentService.UpdateAppointmentAsync(appointmentId, dto);

        //Assert 
        var appointmentFromDb = await _unitOfWorkSqlite.AppointmentRepository.GetByIdAsync(appointmentId);

        appointmentFromDb.Should().NotBeNull();
        appointmentFromDb.ShiftId.Should().Be(newShiftId);
        appointmentFromDb.DoctorId.Should().Be(newDoctorId);
        appointmentFromDb.PatientId.Should().Be(newPatientId);
        appointmentFromDb.StartDate.Should().Be(dto.StartDateTime);
        appointmentFromDb.EndDate.Should().Be(dto.EndDateTime);
        appointmentFromDb.Name.Should().Be(dto.Name);
        appointmentFromDb.Description.Should().Be(dto.Description);
    }

    [Test]
    public async Task GetAppointment_ShouldWorksProper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();

        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };

        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
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
        var appointment = new Appointment()
        {
            Id = appointmentId,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };

        _dbFactory.AddData([appointment]);
        //Act
        var result = await _appointmentService.GetAppointmentAsync(appointmentId);

        //Assert 
        result.Should().NotBeNull();
        result.ShiftId.Should().Be(appointment.ShiftId);
        result.StartDate.Should().Be(appointment.StartDate);
        result.EndDate.Should().Be(appointment.EndDate);
        result.DoctorId.Should().Be(appointment.DoctorId);
        result.PatientId.Should().Be(appointment.PatientId);
    }

    [Test]
    public async Task GetAppointmentsList_ShouldWorksProper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();

        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };

        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
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
        var appointmentId2 = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };

        var appointment2 = new Appointment()
        {
            Id = appointmentId2,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };
        _dbFactory.AddData([appointment, appointment2]);

        var dto = new GetPagedListOfAppointmentDto() { };
        //Act
        var result = await _appointmentService.GetListOfAppointmentsAsync(dto);

        //Assert 
        result.Should().NotBeNull();
        result.Results.Count.Should().Be(2);
        var firstAppointment = result.Results.FirstOrDefault(x => x.Id == appointmentId);
        firstAppointment.Should().NotBeNull();
        firstAppointment.ShiftId.Should().Be(appointment.ShiftId);
        firstAppointment.StartDate.Should().Be(appointment.StartDate);
        firstAppointment.EndDate.Should().Be(appointment.EndDate);
        firstAppointment.DoctorId.Should().Be(appointment.DoctorId);
        firstAppointment.PatientId.Should().Be(appointment.PatientId);

        var secondAppointment = result.Results.FirstOrDefault(x => x.Id == appointmentId2);
        secondAppointment.Should().NotBeNull();
        secondAppointment.ShiftId.Should().Be(appointment2.ShiftId);
        secondAppointment.StartDate.Should().Be(appointment2.StartDate);
        secondAppointment.EndDate.Should().Be(appointment2.EndDate);
        secondAppointment.DoctorId.Should().Be(appointment2.DoctorId);
        secondAppointment.PatientId.Should().Be(appointment2.PatientId);
    }

    [Test]
    public async Task GetAppointmentsList_UsingDatesFilter_ShouldWorksProper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var shiftId = Guid.NewGuid();

        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };

        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
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
        var appointmentId2 = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddDays(25),
            StartDate = DateTime.Now.AddDays(25).AddMinutes(30),
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };

        var appointment2 = new Appointment()
        {
            Id = appointmentId2,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };
        _dbFactory.AddData([appointment, appointment2]);

        var dto = new GetPagedListOfAppointmentDto()
        {
            StartDate = DateTime.Now.AddHours(-1),
            EndDate = DateTime.Now.AddHours(1),
        };
        //Act
        var result = await _appointmentService.GetListOfAppointmentsAsync(dto);

        //Assert 
        result.Should().NotBeNull();
        result.Results.Count.Should().Be(1);

        var resultAppointment = result.Results.FirstOrDefault(x => x.Id == appointmentId2);
        resultAppointment.Should().NotBeNull();
        resultAppointment.ShiftId.Should().Be(appointment2.ShiftId);
        resultAppointment.StartDate.Should().Be(appointment2.StartDate);
        resultAppointment.EndDate.Should().Be(appointment2.EndDate);
        resultAppointment.DoctorId.Should().Be(appointment2.DoctorId);
        resultAppointment.PatientId.Should().Be(appointment2.PatientId);
    }

    [Test]
    public async Task GetAppointmentsList_UsingDatesFilterAndPatientId_ShouldWorksProper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var patientId2 = Guid.NewGuid();
        var shiftId = Guid.NewGuid();

        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };

        var patient2 = new ApplicationUser()
        {
            Id = patientId2,
            UserName = "TestPatient2",
            FirstName = "Patient2",
            LastName = "PatientLastName2",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT2"
        };

        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTDOCTOR"
        };

        await _userManager.CreateAsync(doctor);
        await _userManager.CreateAsync(patient);
        await _userManager.CreateAsync(patient2);


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
        var appointmentId2 = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddDays(25),
            StartDate = DateTime.Now.AddDays(25).AddMinutes(30),
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription"
        };

        var appointment2 = new Appointment()
        {
            Id = appointmentId2,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId2,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription"
        };
        _dbFactory.AddData([appointment, appointment2]);

        var dto = new GetPagedListOfAppointmentDto()
        {
            StartDate = DateTime.Now.AddHours(-1),
            EndDate = DateTime.Now.AddHours(1),
            PatientId = patientId,
        };
        //Act
        var result = await _appointmentService.GetListOfAppointmentsAsync(dto);

        //Assert 
        result.Should().NotBeNull();
        result.Results.Count.Should().Be(0);
    }

    [Test]
    public async Task GetAppointmentsList_PatientId_ShouldWorksProper()
    {
        //Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var patientId2 = Guid.NewGuid();
        var shiftId = Guid.NewGuid();
        var patient = new ApplicationUser()
        {
            Id = patientId,
            UserName = "TestPatient",
            FirstName = "Patient",
            LastName = "PatientLastName",
            Email = "test@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT"
        };

        var patient2 = new ApplicationUser()
        {
            Id = patientId2,
            UserName = "TestPatient2",
            FirstName = "Patient2",
            LastName = "PatientLastName2",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTPATIENT2"
        };

        var doctor = new ApplicationUser()
        {
            Id = doctorId,
            UserName = "TestDcotor",
            FirstName = "DoctorName",
            LastName = "DoctorLastName",
            Email = "test2@test.com",
            ClinicId = TestHelper.ClinicId,
            NormalizedUserName = "TESTDOCTOR"
        };

        await _userManager.CreateAsync(doctor);
        await _userManager.CreateAsync(patient);
        await _userManager.CreateAsync(patient2);


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
        var appointmentId2 = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddDays(25),
            StartDate = DateTime.Now.AddDays(25).AddMinutes(30),
            PatientId = patientId,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };

        var appointment2 = new Appointment()
        {
            Id = appointmentId2,
            DoctorId = doctorId,
            EndDate = DateTime.Now.AddMinutes(30),
            StartDate = DateTime.Now,
            PatientId = patientId2,
            ShiftId = shiftId,
            Name = "TestName",
            Description = "TestDescription",
            ClinicId = _clinicId
        };
        _dbFactory.AddData([appointment, appointment2]);

        var dto = new GetPagedListOfAppointmentDto()
        {
            PatientId = patientId,
        };
        //Act
        var result = await _appointmentService.GetListOfAppointmentsAsync(dto);

        //Assert 
        result.Should().NotBeNull();
        result.Results.Count.Should().Be(1);

        var resultAppointment = result.Results.FirstOrDefault(x => x.Id == appointmentId);
        resultAppointment.Should().NotBeNull();
        resultAppointment.EndDate.Should().Be(appointment.EndDate);
        resultAppointment.StartDate.Should().Be(appointment.StartDate);
        resultAppointment.PatientId.Should().Be(appointment.PatientId);
        resultAppointment.ShiftId.Should().Be(appointment.ShiftId);
    }
}
