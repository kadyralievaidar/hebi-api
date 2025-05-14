using Hebi_Api.Features.Core.Common.RequestHandling;
using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Core.Extensions;

public static class ResponseExtensions
{
    public static ActionResult AsAspNetCoreResult(this Response response)
    {
        if (response.HasError)
            return new ObjectResult(new ApiErrorResponse
            {
                Messages = (response as ErrorResponse)?.Meta,
                MainErrorMessage = response.Exception?.Message
            })
            {
                StatusCode = response.StatusCode
            };
        if (response?.Payload != null)
            return new OkObjectResult(response.Payload);
        return new OkResult();
    }
}
