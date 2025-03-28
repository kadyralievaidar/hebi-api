namespace Hebi_Api.Features.Core.Common.RequestHandling;

public class ErrorResponse : Response
{
    public ErrorResponse(Guid id, Exception? ex = null, int statusCode = 500)
        : base(id)
    {
        StatusCode = statusCode;
        Exception = ex;
        HasError = true;
    }

    public ErrorResponse(Guid id, Dictionary<string, string> messages = null, Exception ex = null, int statusCode = 500)
        : this(id, ex, statusCode)
    {
        Meta = messages;
    }
}
