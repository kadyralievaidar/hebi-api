using FluentAssertions;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.ShiftTemplates.Dtos;
using Hebi_Api.Features.ShiftTemplates.Services;
using Hebi_Api.Tests.UOW;
using NUnit.Framework;
using System.ComponentModel;

namespace Hebi_Api.Tests.ShiftTemplates;

[TestFixture]
public class ShiftTemplateServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IShiftTemplateService _service;

    [SetUp]
    public void Setup()
    {
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _service = new ShiftTemplateService(_unitOfWorkSqlite);
    }

    [Test]
    public async Task CreateShiftTemplate_Should_Insert_New_Template()
    {
        // Arrange
        var dto = new CreateShiftTemplateDto
        {
            Name = "Night Shift",
            StartTime = new TimeOnly(22, 0),
            EndTime = new TimeOnly(6, 0)
        };

        // Act
        await _service.CreateShiftTemplate(dto);

        // Assert
        var templates = await _unitOfWorkSqlite.ShiftTemplateRepository.GetAllAsync();
        templates.Should().ContainSingle();
        templates.First().Name.Should().Be("Night Shift");
    }
    [Test]
    public async Task GetShiftTemplateById_Should_Return_Template()
    {
        // Arrange
        var template = new ShiftTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Day Shift",
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(18, 0),
            ClinicId = TestHelper.ClinicId
        };
        _dbFactory.AddData(new List<ShiftTemplate> { template });

        // Act
        var dto = await _service.GetShiftTemplateById(template.Id);

        // Assert
        dto.Should().NotBeNull();
        dto.Name.Should().Be("Day Shift");
        dto.StartTime.Should().Be(new TimeOnly(9, 0));
        dto.EndTime.Should().Be(new TimeOnly(18, 0));
    }
    [Test]
    public async Task GetShiftTemplates_Should_Return_Paged_List()
    {
        // Arrange
        var templates = new List<ShiftTemplate>
        {
            new() 
            { 
                Id = Guid.NewGuid(), Name = "Morning",
                StartTime = new TimeOnly(6,0), 
                EndTime = new TimeOnly(14,0) , 
                ClinicId = TestHelper.ClinicId
            },
            new() 
            { 
                Id = Guid.NewGuid(), 
                Name = "Evening", 
                StartTime = new TimeOnly(14,0), 
                EndTime = new TimeOnly(22,0), 
                ClinicId = TestHelper.ClinicId 
            }
        };
        _dbFactory.AddData(templates);

        var dto = new GetPagedListOfShiftsTemplatesDto
        {
            PageIndex = 0,
            PageSize = 10,
            SortBy = "Name",
            SortDirection = ListSortDirection.Ascending,
        };

        // Act
        var result = await _service.GetShiftTemplates(dto);

        // Assert
        result.Results.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Results.Select(x => x.Name).Should().Contain(new[] { "Morning", "Evening" });
    }

    [Test]
    public async Task GetShiftTemplates_WithSearchText_Should_Return_Paged_List()
    {
        // Arrange
        var templates = new List<ShiftTemplate>
        {
            new()
            {
                Id = Guid.NewGuid(), Name = "Morning",
                StartTime = new TimeOnly(6,0),
                EndTime = new TimeOnly(14,0) ,
                ClinicId = TestHelper.ClinicId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Evening",
                StartTime = new TimeOnly(14,0),
                EndTime = new TimeOnly(22,0),
                ClinicId = TestHelper.ClinicId
            }
        };
        _dbFactory.AddData(templates);

        var dto = new GetPagedListOfShiftsTemplatesDto
        {
            PageIndex = 0,
            PageSize = 10,
            SortBy = "Name",
            SearchText = "Evening",
            SortDirection = ListSortDirection.Ascending,
        };

        // Act
        var result = await _service.GetShiftTemplates(dto);

        // Assert
        result.Results.Should().HaveCount(1);
        result.TotalCount.Should().Be(2);
        result.Results.FirstOrDefault().Should().NotBeNull();
        result.Results.FirstOrDefault().Name.Should().Be("Evening");
    }
    [Test]
    public async Task UpdateShiftTemplate_Should_Update_Existing_Template()
    {
        // Arrange
        var template = new ShiftTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(16, 0),
            ClinicId= TestHelper.ClinicId
        };
        _dbFactory.AddData(new List<ShiftTemplate> { template });
        _dbFactory.DetachForReload(template);

        var dto = new CreateShiftTemplateDto
        {
            Name = "Updated Name",
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(18, 0)
        };

        // Act
        await _service.UpdateShiftTempalate(template.Id, dto);

        // Assert
        var updated = await _unitOfWorkSqlite.ShiftTemplateRepository.GetByIdAsync(template.Id);
        updated!.Name.Should().Be("Updated Name");
        updated.StartTime.Should().Be(new TimeOnly(10, 0));
        updated.EndTime.Should().Be(new TimeOnly(18, 0));
    }

    [Test]
    public async Task DeleteShiftTemplate_Should_Remove_Template()
    {
        // Arrange
        var template = new ShiftTemplate
        {
            Id = Guid.NewGuid(),
            Name = "ToDelete",
            StartTime = new TimeOnly(7, 0),
            EndTime = new TimeOnly(15, 0),
            ClinicId = TestHelper.ClinicId
        };
        _dbFactory.AddData(new List<ShiftTemplate> { template });
        _dbFactory.DetachForReload(template);

        // Act
        await _service.DeleteShiftTemplate(template.Id);

        // Assert
        var templates = await _unitOfWorkSqlite.ShiftTemplateRepository.GetAllAsync();
        templates.Should().BeEmpty();
    }
}
