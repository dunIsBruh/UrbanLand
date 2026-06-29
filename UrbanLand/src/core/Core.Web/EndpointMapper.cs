using Core.Primitives;
using Microsoft.AspNetCore.Http;

namespace Core.Web;

public static class EndpointMapper
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