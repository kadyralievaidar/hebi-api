using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Diseases.Dtos;

namespace Hebi_Api.Features.Diseases.Mapping;

public class DiseaseProfile : Profile
{
    public DiseaseProfile()
    {
        CreateMap<CreateDiseaseDto, Disease>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}
