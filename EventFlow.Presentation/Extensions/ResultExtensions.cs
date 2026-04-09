using EventFlow.Core.Primitives;

namespace EventFlow.Presentation.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : result.Error.ToActionResult();
    }

    public static IActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess
            ? new OkResult()
            : result.Error.ToActionResult();
    }

    private static IActionResult ToActionResult(this Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(new { error.Code, error.Message }),
            ErrorType.Validation => new BadRequestObjectResult(new { error.Code, error.Message }),
            ErrorType.Conflict => new ConflictObjectResult(new { error.Code, error.Message }),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error.Code, error.Message }),
            ErrorType.Forbidden => new ObjectResult(new { error.Code, error.Message })
            {
                StatusCode = StatusCodes.Status403Forbidden
            },
            _ => new ObjectResult(new { error.Code, error.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}

public static class ResultTaskExtensions
{
    public static async Task<IActionResult> ToActionResultAsync<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.ToActionResult();
    }

    public static async Task<IActionResult> ToActionResultAsync(this Task<Result> resultTask)
    {
        var result = await resultTask;
        return result.ToActionResult();
    }
}
