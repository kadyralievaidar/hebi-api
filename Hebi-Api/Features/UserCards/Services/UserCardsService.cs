using AutoMapper;
using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.Services;

public class UserCardsService : IUserCardsService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserCardsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteUserCard(Guid id)
    {
        var userCard = await _unitOfWork.UserCardsRepository.GetByIdAsync(id);
        _unitOfWork.UserCardsRepository.Delete(userCard);
        await _unitOfWork.SaveAsync();
    }

    public async Task<PagedResult<UserCardResponseDto>> GetListOfUserCardsAsync(GetPagedListOfUserCardDto dto)
    {
        var result = await _unitOfWork.UserCardsRepository.GetUserCards(dto);
        return result;
    }

    public async Task<UserCard> GetUserCardAsync(Guid id)
    {
        var userCard = await _unitOfWork.UserCardsRepository.FirstOrDefaultAsync(x => x.Id == id, relations: ["Appointments"]);
        return userCard;
    }
    public async Task<Guid> CreateUserCard(CreateUserCardDto dto)
    {
        var userCard = new UserCard() 
        {
            PatientId = dto.UserId,
        };
        await _unitOfWork.UserCardsRepository.InsertAsync(userCard);
        await _unitOfWork.SaveAsync();
        return userCard.Id;
    }
}
