using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.UserCards.Dtos;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class UserCardsRepository : GenericRepository<UserCard>, IUserCardsRepository
{
    private readonly HebiDbContext _dbContext;
    public UserCardsRepository(HebiDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
        _dbContext = context;
    }
    public async Task<PagedResult<UserCardResponseDto>> GetUserCards(GetPagedListOfUserCardDto dto)
    {
        var query = _dbContext.PatientCard
            .AsNoTracking()
            .Include(x => x.Patient)
            .Include(x => x.Appointments)!
                .ThenInclude(a => a.Disease)
            .Where(x => !x.IsDeleted)
            .Select(usercard => new UserCardResponseDto
            {
                UserInfo = new BasicInfoDto
                {
                    UserId = usercard.PatientId,
                    FirstName = usercard.Patient.FirstName ?? "",
                    LastName = usercard.Patient.LastName,
                    PhoneNumber = usercard.Patient.PhoneNumber,
                    Email = usercard.Patient.Email
                },
                Appointments = usercard.Appointments != null
                    ? usercard.Appointments.Select(appt => new AppointmentDto
                    {
                        DiseaseName = appt.Disease != null ? appt.Disease.Name : null,
                        EndDate = appt.EndDate,
                        StartDate = appt.StartDate,
                        Price = appt.Disease != null ? appt.Disease.PriceWithDiscount : 0
                    }).ToList()
                    : new List<AppointmentDto>()
            });

        var count = await query.CountAsync();
        var results = await query
            .Skip(dto.PageSize * dto.PageIndex)
            .Take(dto.PageSize)
            .ToListAsync();

        return new PagedResult<UserCardResponseDto>
        {
            Results = results,
            TotalCount = count
        };
    }
}
