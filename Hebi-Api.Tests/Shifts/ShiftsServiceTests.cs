using FluentAssertions;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Shifts.Dtos;
using Hebi_Api.Features.Shifts.Services;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace Hebi_Api.Tests.Shifts;
public class ShiftsServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IShiftsService _service;
    private Mock<IHttpContextAccessor> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = TestHelper.CreateHttpContext(); // use user identifier mock
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _service = new ShiftsService(_unitOfWorkSqlite, _mock.Object);
    }
    [Test]
    public async Task CreateShift_Should_Create_And_Return_Id()
    {
        // Arrange
        var shiftIddb = Guid.NewGuid();
        _dbFactory.AddData(new List<Shift>() { new() { Id = shiftIddb, ClinicId = TestHelper.ClinicId } });
        var appointment = new Appointment { Id = Guid.NewGuid(), ClinicId = TestHelper.ClinicId, ShiftId = shiftIddb };
        var doctor = new ApplicationUser { Id = Guid.NewGuid(), ClinicId = TestHelper.ClinicId };

        _dbFactory.AddData(new List<ApplicationUser>() { doctor});
        // Insert the appointment using the same UnitOfWork
        _dbFactory.AddData(new List<Appointment>() { appointment});

        var dto = new CreateShiftDto
        {
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(4),
            AppointmentIds = new List<Guid> { appointment.Id },
            DoctorId = doctor.Id
        };

        _unitOfWorkSqlite.DetachForReload(appointment);
        _unitOfWorkSqlite.DetachForReload(doctor);
        // Act
        var shiftId = await _service.CreateShift(dto);
        // Assert
        var shift = await _unitOfWorkSqlite.ShiftsRepository.GetByIdAsync(shiftId);
        shift.Should().NotBeNull();
        shift.Appointments.Count().Should().Be(1);
    }

    [Test]
    public async Task DeleteShift_Should_Remove_The_Shift()
    {
        // Arrange
        var shift = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(2),
            ClinicId=TestHelper.ClinicId
        };
        await _unitOfWorkSqlite.ShiftsRepository.InsertAsync(shift);
        await _unitOfWorkSqlite.SaveAsync();

        // Act
        await _service.DeleteShift(shift.Id);

        // Assert
        var deleted = await _unitOfWorkSqlite.ShiftsRepository.GetByIdAsync(shift.Id);
        deleted.Should().BeNull();  
    }

    [Test]
    public async Task GetShiftAsync_Should_Return_Shift()
    {
        // Arrange
        var shift = new Shift
        {
            Id = Guid.NewGuid(),
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(2),
            ClinicId = TestHelper.ClinicId
        };
        await _unitOfWorkSqlite.ShiftsRepository.InsertAsync(shift);
        await _unitOfWorkSqlite.SaveAsync();

        // Act
        var result = await _service.GetShiftAsync(shift.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(shift.Id);
    }

    [Test]
    public async Task GetListOfShiftsAsync_Should_Filter_And_Paginate()
    {
        // Arrange
        var now = DateTime.Now;
        var shift1 = new Shift { StartTime = now, EndTime = now.AddHours(2), ClinicId=TestHelper.ClinicId };
        var shift2 = new Shift { StartTime = now.AddDays(1), EndTime = now.AddDays(1).AddHours(2), ClinicId = TestHelper.ClinicId };

        await _unitOfWorkSqlite.ShiftsRepository.InsertAsync(shift1);
        await _unitOfWorkSqlite.ShiftsRepository.InsertAsync(shift2);
        await _unitOfWorkSqlite.SaveAsync();

        var dto = new GetPagedListOfShiftsDto
        {
            StartTime = now,
            EndTime = now.AddDays(2),
            PageIndex = 0,
            PageSize = 10,
            SortBy = nameof(Shift.StartTime),
            SortDirection = System.ComponentModel.ListSortDirection.Ascending
        };

        // Act
        var result = await _service.GetListOfShiftsAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
    }

    [Test]
    public async Task UpdateShift_Should_Update_Shift()
    {
        // Arrange
        var shift = new Shift
        {
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            ClinicId = TestHelper.ClinicId
        };
        _dbFactory.AddData(new List<Shift>() { shift });

        var newDto = new CreateShiftDto
        {
            StartTime = DateTime.Now.AddHours(1),
            EndTime = DateTime.Now.AddHours(3),
            AppointmentIds = new List<Guid>()
        };

        // Act
        var updated = await _service.UpdateShift(shift.Id, newDto);

        // Assert
        updated.Should().NotBeNull();
        updated.StartTime.Should().Be(newDto.StartTime);
        updated.EndTime.Should().Be(newDto.EndTime);
    }
}
