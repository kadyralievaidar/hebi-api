using AutoMapper;
using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Clinics.Mapping;

public class ClinicProfile : Profile
{
    public ClinicProfile()
    {
        CreateMap<CreateClinicDto, Clinic>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
          .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}
