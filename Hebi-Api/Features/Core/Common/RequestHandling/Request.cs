namespace Hebi_Api.Features.Core.Common.RequestHandling;

/// <summary>
///     Abstract request
/// </summary>
public abstract class Request : MediatR.IRequest<Response>
{
    /// <summary>
    ///     A request id that should be used as an idempontency code
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     Any other necessary information for the Request
    /// </summary>
    public Dictionary<string, string> Meta { get; set; } = new();

    /// <summary>
    ///     Gets the request date and time
    /// </summary>
    public DateTime RequestDate { get; } = DateTime.UtcNow;

    /// <summary>
    ///     Request path
    /// </summary>
    public string Path { get; set; }
}

/// <summary>
///     Represents a request made to Delphinium to process and return a response.
/// </summary>
/// <typeparam name="T">The type of the Request payload</typeparam>
public abstract class Request<T> : Request
{
    /// <summary>
    ///     The payload of the request
    /// </summary>
    public T Payload { get; set; }
}
