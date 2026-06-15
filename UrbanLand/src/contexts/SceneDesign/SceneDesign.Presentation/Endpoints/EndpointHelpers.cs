using Microsoft.AspNetCore.Http;
using SharedKernel.Primitives;

namespace SceneDesign.Presentation.Endpoints;

public static class EndpointHelpers
{
    public static IResult MapErrorToResponse(Error error)
    {
        return error.Code switch
        {
            "NotFound" or "Forbidden" => TypedResults.NotFound(new Error(error.Code, error.Message)),
            _ => TypedResults.BadRequest(new Error(error.Code, error.Message))
        };
    }
}
