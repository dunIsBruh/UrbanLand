using Microsoft.AspNetCore.Http;
using SharedKernel.Primitives;

namespace AssetCatalog.Presentation.Endpoints;

public static class EndpointHelpers
{
    public static Guid GetUserId(HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null && Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public static IResult MapErrorToResponse(Error error)
    {
        return error.Code switch
        {
            "NotFound" or "Forbidden" => TypedResults.NotFound(new Error(error.Code, error.Message)),
            _ => TypedResults.BadRequest(new Error(error.Code, error.Message))
        };
    }
}
