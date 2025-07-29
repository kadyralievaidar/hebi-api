using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.Core.DataAccess.Interfaces;

public interface IUserCardsRepository : IGenericRepository<UserCard>
{
    /// <summary>
    ///     Return paged result of user card dto
    /// </summary>
    /// <returns></returns>
   Task<PagedResult<UserCardResponseDto>> GetUserCards(GetPagedListOfUserCardDto dto);
}
