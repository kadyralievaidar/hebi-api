using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class AppointmentsRepository : GenericRepository<Appointment>, IAppointmentsRepository
{
    public AppointmentsRepository(HebiDbContext context) : base(context)
    {
    }
}
