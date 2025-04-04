using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Diseases.Services;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;

namespace Hebi_Api.Tests.Diseases;
public class DiseasesServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IDiseaseService _service;
    private Mock<IHttpContextAccessor> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<IHttpContextAccessor>();
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _service = new DiseasesService(_unitOfWorkSqlite, TestHelper.CreateMapper(typeof(Program)),
            _mock.Object);
    }
}
