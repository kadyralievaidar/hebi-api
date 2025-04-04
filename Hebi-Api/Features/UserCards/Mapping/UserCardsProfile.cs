using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.Mapping;

public class UserCardsProfile : Profile
{
    public UserCardsProfile()
    {
        CreateMap<CreateUserCardDto, UserCard>()
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.UserId));
    }
}
