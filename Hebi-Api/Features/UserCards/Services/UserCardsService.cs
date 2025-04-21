using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.Services;

public class UserCardsService : IUserCardsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserCardsService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public async Task DeleteUserCard(Guid id)
    {
        var userCard = await _unitOfWork.UserCardsRepository.GetByIdAsync(id)
            ?? throw new NullReferenceException(nameof(UserCard));
        _unitOfWork.UserCardsRepository.Delete(userCard);
        await _unitOfWork.SaveAsync();
    }

    public async Task<List<UserCard>> GetListOfUserCardsAsync(GetPagedListOfUserCardDto dto)
    {
        var userCards = await _unitOfWork.UserCardsRepository.SearchAsync(x => true, dto.SortBy, dto.SortDirection,
                                                                    dto.PageIndex * dto.PageSize,
                                                                    dto.PageSize);
        return userCards;
    }

    public async Task<UserCard> GetUserCardAsync(Guid id)
    {
        var userCard = await _unitOfWork.UserCardsRepository.GetByIdAsync(id)
                        ?? throw new NullReferenceException(nameof(UserCard));
        return userCard;
    }

    public async Task<UserCard> UpdateUserCard(Guid id, CreateUserCardDto dto)
    {
        var userCard = await _unitOfWork.UserCardsRepository.GetByIdAsync(id)
                ?? throw new NullReferenceException(nameof(UserCard));

        userCard = new UserCard() { };
        _unitOfWork.UserCardsRepository.Update(userCard);
        await _unitOfWork.SaveAsync();
        return userCard;
    }

    public async Task<Guid> CreateUserCard(CreateUserCardDto dto)
    {
        var userCard = new UserCard() { };
        await _unitOfWork.UserCardsRepository.InsertAsync(userCard);
        await _unitOfWork.SaveAsync();
        return userCard.Id;
    }
}
