using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.UOW;

namespace Hebi_Api.Features.Diseases.Services;

public class DiseasesService : IDiseaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;

    public DiseasesService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }
}
