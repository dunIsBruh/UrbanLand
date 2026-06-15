using Microsoft.AspNetCore.Http;
using SharedKernel.Primitives;

namespace ProjectManagement.Presentation.Endpoints;

public static class EndpointHelpers
{
    public static IResult MapErrorToResponse(Error error)
    {
        return error.Code switch
        {
            "NotFound" => TypedResults.NotFound(new Error(error.Code, error.Message)),
            "Forbidden" => TypedResults.Forbid(),
            _ => TypedResults.BadRequest(new Error(error.Code, error.Message))
        };
    }
}
