using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Tests.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;

namespace Hebi_Api.Tests.Appoitments;

[TestFixture]

public class AppointmentServiceTests
{
    private UnitOfWorkFactory _dbFactory;
    private IUnitOfWork _unitOfWorkSqlite;
    private IAppointmentsService _appointmentService;
    private Mock<IHttpContextAccessor> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<IHttpContextAccessor>();
        _dbFactory = new UnitOfWorkFactory();
        _unitOfWorkSqlite = _dbFactory.CreateUnitOfWork(true);
        _appointmentService = new AppointmentsService(_unitOfWorkSqlite, TestHelper.CreateMapper(typeof(Program)),
            _mock.Object);
    }

    [Test]
    public async Task CreateAppointment_Should_Work_Proper()
    {

    }
}
