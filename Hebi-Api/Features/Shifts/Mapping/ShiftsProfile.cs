using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.Mapping;

public class ShiftsProfile : Profile
{
    public ShiftsProfile()
    {
        CreateMap<CreateShiftDto, Shift>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId ?? Guid.Empty));
    }
}
