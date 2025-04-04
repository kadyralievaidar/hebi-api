using AutoMapper;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Appointments.Mapping;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId ?? Guid.Empty))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId ?? Guid.Empty))
            .ForMember(dest => dest.PatientShortName, opt => opt.MapFrom(src => src.ShortName))
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
            .ForMember(dest => dest.ShiftId, opt => opt.MapFrom(src => src.ShiftId ?? Guid.Empty))
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Doctor, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore());

        CreateMap<UpdateAppointmentDto, Appointment>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId ?? Guid.Empty))
            .ForMember(dest => dest.ShiftId, opt => opt.MapFrom(src => src.ShiftId ?? Guid.Empty))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId ?? Guid.Empty));
    }
}
