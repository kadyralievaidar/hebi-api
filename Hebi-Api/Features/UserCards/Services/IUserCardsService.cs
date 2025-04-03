using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.Services;

public interface IUserCardsService
{
    Task<Guid> CreateUserCard(CreateUserCardDto dto);
    Task DeleteUserCard(Guid id);
    Task<UserCard> UpdateUserCard(Guid id, CreateUserCardDto dto);
    Task<List<UserCard>> GetListOfUserCardsAsync(GetPagedListOfUserCardDto dto);
    Task<UserCard> GetUserCardAsync(Guid id);
}
