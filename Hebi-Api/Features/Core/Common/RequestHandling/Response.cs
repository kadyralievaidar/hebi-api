using System.Text.Json.Serialization;

namespace Hebi_Api.Features.Core.Common.RequestHandling;

public class Response
{
    protected bool _hasError;

    public Guid Id { get; protected set; }

    public int StatusCode { get; set; }

    [JsonIgnore]
    public Type PayloadType => Payload?.GetType();

    public object Payload { get; set; }

    [JsonIgnore]
    public Exception Exception { get; set; }

    public IList<string> Messages { get; set; } = new List<string>();


    public bool HasError
    {
        get
        {
            if (Exception == null)
            {
                return _hasError;
            }

            return true;
        }
        set
        {
            _hasError = value;
        }
    }

    public Dictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();


    public Response(Guid id)
    {
        Id = id;
    }

    public static Response Accepted(Guid id, object payload = null)
    {
        return new SuccessResponse(id, payload, 202);
    }

    public static Response BadRequest(Guid id, Exception ex = null, Dictionary<string, string> messages = null)
    {
        return new ErrorResponse(id, messages, ex, 400);
    }

    public static Response Conflict(Guid id, Exception ex = null)
    {
        return new ErrorResponse(id, ex, 409);
    }

    public static Response Created(Guid id, object payload = null)
    {
        return new SuccessResponse(id, payload, 201);
    }

    public static Response Forbidden(Guid id, Exception ex = null)
    {
        return new ErrorResponse(id, ex, 403);
    }

    public static Response InternalServerError(Guid id, Exception ex = null)
    {
        return new ErrorResponse(id, ex);
    }

    public static Response NotFound(Guid id, Exception ex = null)
    {
        return new ErrorResponse(id, ex, 404);
    }

    public static Response Ok(Guid id, object payload = null)
    {
        return new SuccessResponse(id, payload);
    }

    public static Response TooManyRequests(Guid id, Exception ex = null)
    {
        return new ErrorResponse(id, ex, 429);
    }

    public static Response Unauthorized(Guid id, Exception ex = null)
    {
        return new ErrorResponse(id, ex, 401);
    }

    public static Response FilePath(Guid id, string filePath)
    {
        return new SuccessResponse(id, filePath);
    }
}
