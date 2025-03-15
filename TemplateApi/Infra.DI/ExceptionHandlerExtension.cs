using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Extensions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Reflection;
using FluentValidation;
using System.Net;

namespace Infra.DI
{
    public class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        private static Dictionary<string, object?> LoadRequestHeaders(HttpContext httpContext)
        {
            Dictionary<string, object?> headers = httpContext.Request.Headers
                .Select(item => ((string)item.Key, (object?)item.Value))
                .ToDictionary();

            headers.Add("datetime", DateTime.Now.ToString("G"));

            return headers;
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
                    Title = ((HttpStatusCode)responseCode).GetDisplayName(),
                    Type = exception.GetType().Name,
                    Detail = exception.Message,
                    Extensions = LoadRequestHeaders(httpContext)
                }
            };
        }

        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
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

            return problemDetailsService.TryWriteAsync(problemDetailsContext);
        }
    }

    public static class ExceptionHandlerExtension
    {
        private static string GetRequestPath(ProblemDetailsContext context)
        {
            return $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        }

        public static IServiceCollection AddExceptionHandler(this IServiceCollection services) => services
            .AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = GetRequestPath(context);

                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            })
            .AddExceptionHandler<CustomExceptionHandler>();
    }
}
