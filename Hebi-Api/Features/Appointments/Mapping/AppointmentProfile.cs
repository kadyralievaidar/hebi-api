using AutoMapper;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Appointments.Mapping;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<CreateAppointmentDto, Appointment>();
    }
}
