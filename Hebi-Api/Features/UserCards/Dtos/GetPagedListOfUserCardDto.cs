using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.UserCards.Dtos;

public class GetPagedListOfUserCardDto : BaseListRequest
{
    public DateTime StartTimeRange {  get; set; }
    public DateTime EndTimeRange { get; set; }
}
