using FluentAssertions;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.UserCards.Dtos;
using Hebi_Api.Features.UserCards.Services;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;

namespace Hebi_Api.Tests.UserCards;
public class UserCardsServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IUserCardsService _service;

    [SetUp]
    public void Setup()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _service = new UserCardsService(_unitOfWorkSqlite);
    }

    [Test]
    public async Task CreateUserCard_ShouldWorkProperly()
    {
        var patient = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            ClinicId = TestHelper.ClinicId,
            UserName = "DrTwo",
            Email = "drtwo@example.com",
            FirstName = "Name2",
            LastName = "LastName2",
            PhoneNumber = "555555555",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        _dbFactory.AddData(new List<ApplicationUser>() { patient});
        //Act
        await _service.CreateUserCard(new CreateUserCardDto()
        {
            UserId = patient.Id,
        });

        //Assert
        var userCards = await _unitOfWorkSqlite.UserCardsRepository.GetAllAsync();
        var userCard = userCards.FirstOrDefault();
        userCard.Should().NotBeNull();
        userCard.PatientId = patient.Id;
    }

    [Test]
    public async Task GetAllUserCards()
    {
        var patient = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            ClinicId = TestHelper.ClinicId,
            UserName = "DrTwo",
            Email = "drtwo@example.com",
            FirstName = "Name2",
            LastName = "LastName2",
            PhoneNumber = "555555555",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var patient2 = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            ClinicId = TestHelper.ClinicId,
            UserName = "DrTwo",
            Email = "drtwo@example.com",
            FirstName = "Name2",
            LastName = "LastName2",
            PhoneNumber = "555555555",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        _dbFactory.AddData(new List<ApplicationUser>() { patient, patient2 });
        _dbFactory.AddData(new List<UserCard>()
        {
            new()
            {
                PatientId = patient.Id,
                ClinicId= TestHelper.ClinicId,
            },
            new()
            {
                PatientId = patient2.Id,
                ClinicId = TestHelper.ClinicId
            }
        });
        //Act
        var dto = new GetPagedListOfUserCardDto();
        var result = await _service.GetListOfUserCardsAsync(dto);

        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        var patientIds = result.Select(x => x.PatientId);
        patientIds.Should().Contain(patient.Id);
        patientIds.Should().Contain(patient2.Id);
    }

    [Test]
    public async Task GetUserCardById()
    {
        var patient = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            ClinicId = TestHelper.ClinicId,
            UserName = "DrTwo",
            Email = "drtwo@example.com",
            FirstName = "Name2",
            LastName = "LastName2",
            PhoneNumber = "555555555",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var patient2 = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            ClinicId = TestHelper.ClinicId,
            UserName = "DrTwo",
            Email = "drtwo@example.com",
            FirstName = "Name2",
            LastName = "LastName2",
            PhoneNumber = "555555555",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        _dbFactory.AddData(new List<ApplicationUser>() { patient, patient2 });
        var userCardId = Guid.NewGuid();
        _dbFactory.AddData(new List<UserCard>()
        {
            new()
            {
                Id = userCardId,
                PatientId = patient.Id,
                ClinicId= TestHelper.ClinicId,
                Appointments = new List<Appointment>()
                {
                    new()
                    {
                        ClinicId = TestHelper.ClinicId,
                    },
                    new()
                    {
                        ClinicId = TestHelper.ClinicId
                    }
                }
            },
            new()
            {
                PatientId = patient2.Id,
                ClinicId = TestHelper.ClinicId
            }
        });
        //Act
        var result = await _service.GetUserCardAsync(userCardId);

        result.Should().NotBeNull();
        result.Id.Should().Be(userCardId);
        result.Appointments.Should().HaveCount(2);
    }
    [Test]
    public async Task DeleteUserCard()
    {
        var patient = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            ClinicId = TestHelper.ClinicId,
            UserName = "DrTwo",
            Email = "drtwo@example.com",
            FirstName = "Name2",
            LastName = "LastName2",
            PhoneNumber = "555555555",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        _dbFactory.AddData(new List<ApplicationUser>() { patient });
        var userCardId = Guid.NewGuid();
        _dbFactory.AddData(new List<UserCard>()
        {
            new()
            {
                Id = userCardId,
                PatientId = patient.Id,
                ClinicId= TestHelper.ClinicId,
            }
        });
        //Act
        await _service.DeleteUserCard(userCardId);

        var result = await _unitOfWorkSqlite.UserCardsRepository.GetByIdAsync(userCardId);
        result.Should().BeNull();
    }
}
