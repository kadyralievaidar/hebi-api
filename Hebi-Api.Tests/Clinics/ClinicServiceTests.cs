using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;

namespace Hebi_Api.Tests.Clinics;
public class ClinicServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IClinicsService _service;
    private Mock<IHttpContextAccessor> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<IHttpContextAccessor>();
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _service = new ClinicsService(_unitOfWorkSqlite, TestHelper.CreateMapper(typeof(Program)),
            _mock.Object);
    }
}
