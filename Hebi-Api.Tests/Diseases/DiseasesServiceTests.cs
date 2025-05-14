using FluentAssertions;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Diseases.Dtos;
using Hebi_Api.Features.Diseases.Services;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.ComponentModel;

namespace Hebi_Api.Tests.Diseases;

[TestFixture]
public class DiseasesServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IDiseaseService _service;
    private Mock<IHttpContextAccessor> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = TestHelper.CreateHttpContext(); // use user identifier mock
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _service = new DiseasesService(_unitOfWorkSqlite, _mock.Object);
    }

    [Test]
    public async Task CreateDisease_ShouldWorkProperly()
    {
        // Arrange
        var dto = new CreateDiseaseDto
        {
            Name = "TestDisease",
            Description = "Test Description"
        };

        // Act
        var result = await _service.CreateDisease(dto);

        // Assert
        var disease = await _unitOfWorkSqlite.DiseaseRepository.GetByIdAsync(result);
        disease.Should().NotBeNull();
        disease.Name.Should().Be(dto.Name);
        disease.Description.Should().Be(dto.Description);
        disease.CreatedBy.Should().Be(_mock.Object.GetUserIdentifier());
    }

    [Test]
    public async Task DeleteDisease_ShouldRemoveDisease()
    {
        // Arrange
        var disease = new Disease
        {
            Id = Guid.NewGuid(),
            Name = "ToDelete",
            Description = "To be deleted"
        };
        _dbFactory.AddData(new List<Disease> { disease });

        // Act
        await _service.DeleteDisease(disease.Id);

        // Assert
        var result = await _unitOfWorkSqlite.DiseaseRepository.GetByIdAsync(disease.Id);
        result.Should().BeNull();
    }

    [Test]
    public void DeleteDisease_WithInvalidId_ShouldThrow()
    {
        // Act
        Func<Task> act = async () => await _service.DeleteDisease(Guid.NewGuid());

        // Assert
        act.Should().ThrowAsync<NullReferenceException>()
           .WithMessage("Disease");
    }

    [Test]
    public async Task GetDiseaseAsync_ShouldReturnDisease()
    {
        // Arrange
        var disease = new Disease
        {
            Id = Guid.NewGuid(),
            Name = "ToGet",
            Description = "Details"
        };
        _dbFactory.AddData(new List<Disease> { disease });

        // Act
        var result = await _service.GetDiseaseAsync(disease.Id);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(disease.Name);
    }

    [Test]
    public void GetDiseaseAsync_WithInvalidId_ShouldThrow()
    {
        // Act
        Func<Task> act = async () => await _service.GetDiseaseAsync(Guid.NewGuid());

        // Assert
        act.Should().ThrowAsync<NullReferenceException>()
           .WithMessage("Disease");
    }

    [Test]
    public async Task UpdateDisease_ShouldModifyFieldsCorrectly()
    {
        // Arrange
        var disease = new Disease
        {
            Id = Guid.NewGuid(),
            Name = "Original",
            Description = "Original Desc"
        };
        _dbFactory.AddData(new List<Disease> { disease });

        var dto = new CreateDiseaseDto
        {
            Name = "Updated",
            Description = "Updated Desc"
        };

        // Act
        var result = await _service.UpdateDisease(disease.Id, dto);

        // Assert
        result.Name.Should().Be(dto.Name);
        result.Description.Should().Be(dto.Description);
        result.LastModifiedBy.Should().Be(_mock.Object.GetUserIdentifier());
    }

    [Test]
    public async Task GetListOfDiseasesAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        _dbFactory.AddData(new List<Disease>
        {
            new() { Id = Guid.NewGuid(), Name = "Flu", Description = "Cold virus", ClinicId = TestHelper.ClinicId },
            new() { Id = Guid.NewGuid(), Name = "Covid", Description = "Coronavirus", ClinicId = TestHelper.ClinicId },
            new() { Id = Guid.NewGuid(), Name = "Allergy", Description = "Reaction", ClinicId = TestHelper.ClinicId }
        });

        var dto = new GetPagedListOfDiseaseDto
        {
            PageIndex = 0,
            PageSize = 10,
            SearchText = "co", // should match "Covid" and "Cold"
            SortBy = "Name",
            SortDirection = ListSortDirection.Ascending,
        };

        // Act
        var result = await _service.GetListOfDiseasesAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainSingle(x => x.Name == "Covid");
    }
}
