
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Extensions;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace Infra.DI
{
    public class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        private static Dictionary<string, object?> LoadRequestHeaders(HttpContext httpContext)
        {
            return httpContext.Request.Headers
                .Select(item => ((string)item.Key, (object?)item.Value))
                .ToDictionary();
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

            IDictionary<string, object?> headers = LoadRequestHeaders(httpContext);

            headers.Add("datetime", DateTime.Now.ToString("G"));

            ProblemDetails problemDetails = new()
            {
                Status = responseCode,
                Title = ((HttpStatusCode)responseCode).GetDisplayName(),
                Type = exception.GetType().Name,
                Detail = exception.Message,
                Extensions = headers
            };

            return problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                Exception = exception,
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
        }
    }

    public static class ExceptionHandlerExtension
    {
        public static IServiceCollection AddExceptionHandler(this IServiceCollection services) => services
            .AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance =
                        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            })
            .AddExceptionHandler<CustomExceptionHandler>();
    }
}
