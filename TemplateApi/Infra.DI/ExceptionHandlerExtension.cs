using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Reflection;
using FluentValidation;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Infra.DI
{
    public class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        private static Dictionary<string, object?> LoadRequestHeaders(HttpContext httpContext)
        {
            return httpContext.Request.Headers
                 .Select(item => (item.Key, (object?)item.Value))
                 .ToDictionary();
        }

        private static ProblemDetailsContext GetProblemDetailsContext(HttpContext httpContext, Exception exception, int responseCode)
        {
            return new()
            {
                Exception = exception,
                HttpContext = httpContext,
                ProblemDetails = new()
                {
                    Status = responseCode,
                    Title = ((HttpStatusCode)responseCode).ToString(),
                    Type = exception.GetType().Name,
                    Detail = exception.Message,
                    Extensions = LoadRequestHeaders(httpContext)
                }
            };
        }

        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<CustomExceptionHandler>>();

            int responseCode = exception.GetType().Name switch
            {
                nameof(ValidationException) => (int)HttpStatusCode.BadRequest,
                nameof(ArgumentNullException) => (int)HttpStatusCode.BadRequest,
                nameof(IndexOutOfRangeException) => (int)HttpStatusCode.BadRequest,
                nameof(InvalidFilterCriteriaException) => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            httpContext.Response.StatusCode = responseCode;

            ProblemDetailsContext problemDetailsContext = GetProblemDetailsContext(httpContext, exception, responseCode);

            logger.LogError("An Error Has Ocurred, See The Details: {ProblemDetails}", problemDetailsContext);

            return problemDetailsService.TryWriteAsync(problemDetailsContext);
        }
    }

    public static class ExceptionHandlerExtension
    {
        public static IServiceCollection AddExceptionHandler(this IServiceCollection services) => services
            .AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

                    context.ProblemDetails.Extensions.TryAdd("Method", context.HttpContext.Request.Method);
                    context.ProblemDetails.Extensions.TryAdd("Path", context.HttpContext.Request.Path);
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    context.ProblemDetails.Extensions.TryAdd("datetime", DateTime.UtcNow);
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            }).AddExceptionHandler<CustomExceptionHandler>();
    }
}
