namespace Hebi_Api.Features.Core.Common.RequestHandling;

public class ErrorResponse : Response
{
    public ErrorResponse(Guid id, Exception? ex = null, int statusCode = 500)
        : base(id)
    {
        StatusCode = statusCode;
        Exception = ex!;
        HasError = true;
    }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public ErrorResponse(Guid id, Dictionary<string, string> messages = null, Exception ex = null, int statusCode = 500)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        : this(id, ex, statusCode)
    {
        Meta = messages;
    }
}
