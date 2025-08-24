// Web/ResultToHttp.cs
using Microsoft.AspNetCore.Mvc;

public static class ResultToHttp
{
    // Bas: 200/400/403/404/409/500
    public static (int status, object payload) ToHttp<T>(ServiceResult<T> result)
        => ToHttp(result, successStatus: 200);

    // Ange success-status (t.ex. 201 vid POST, 204 vid DELETE utan payload)
    public static (int status, object payload) ToHttp<T>(ServiceResult<T> result, int successStatus)
    {
        if (result.IsSuccess)
        {
            if (successStatus == 204) // no content
                return (204, new { }); // tom payload för konsekvens
            return (successStatus, result.Value!);
        }

        var status =
            result.Errors.Any(e => e.Kind == ErrorKind.Unexpected) ? 500 :
            result.Errors.Any(e => e.Kind == ErrorKind.Validation) ? 400 :
            result.Errors.Any(e => e.Kind == ErrorKind.Forbidden)  ? 403 :
            result.Errors.Any(e => e.Kind == ErrorKind.NotFound)   ? 404 : 409;

        // Minimal ProblemDetails (RFC 7807-kompatibel)
        var problem = new ProblemDetails
        {
            Status = status,
            Title  = PickTitle(status),
            Type   = $"https://httpstatuses.com/{status}",
            Detail = result.Errors.FirstOrDefault()?.Message
        };

        // Lägg med hela fel-listan också
        var payload = new
        {
            problem.Type,
            problem.Title,
            problem.Status,
            problem.Detail,
            errors = result.Errors.Select(e => new { e.Code, e.Message, e.Field })
        };

        return (status, payload);
    }

    private static string PickTitle(int status) => status switch
    {
        400 => "Validation error",
        403 => "Forbidden",
        404 => "Not found",
        409 => "Conflict",
        500 => "Unexpected error",
        _   => "Error"
    };

    // Snygga extension helpers för controllers
    public static IActionResult ToActionResult<T>(this ControllerBase c, ServiceResult<T> result, int successStatus = 200)
    {
        var (status, payload) = ToHttp(result, successStatus);
        return c.StatusCode(status, payload);
    }
}