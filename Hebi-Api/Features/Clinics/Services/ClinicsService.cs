using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Clinics.Services;

public class ClinicsService : IClinicsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;

    public ClinicsService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }
}
