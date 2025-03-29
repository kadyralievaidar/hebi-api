using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

public class CreateShiftRequest : Request<Response>
{
    public CreateShiftDto CreateShiftDto { get; set; }
    public CreateShiftRequest(CreateShiftDto createShiftDto)
    {
        CreateShiftDto = createShiftDto;
    }
}
