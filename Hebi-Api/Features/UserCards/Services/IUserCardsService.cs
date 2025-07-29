using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.Services;

/// <summary>
///     User card service
/// </summary>
public interface IUserCardsService
{
    /// <summary>
    ///     Create user card
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateUserCard(CreateUserCardDto dto);

    /// <summary>
    ///     Delete user card
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteUserCard(Guid id);

    /// <summary>
    ///     Get paginated list of user cards
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagedResult<UserCardResponseDto>> GetListOfUserCardsAsync(GetPagedListOfUserCardDto dto);

    /// <summary>
    ///     Get user card by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserCard> GetUserCardAsync(Guid id);
}
