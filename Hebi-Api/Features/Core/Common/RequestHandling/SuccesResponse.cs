namespace Hebi_Api.Features.Core.Common.RequestHandling;

public class SuccessResponse : Response
{
    public SuccessResponse(Guid id, object? payload = null, int statusCode = 200)
        : base(id)
    {
        StatusCode = statusCode;
        Payload = payload;
        HasError = false;
    }
}