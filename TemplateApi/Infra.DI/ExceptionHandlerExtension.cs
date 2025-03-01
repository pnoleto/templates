using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class ExceptionHandlerExtension
    {
        public static IServiceCollection AddExceptionHandler(this IServiceCollection services) => services.AddExceptionHandler(options =>
        {
            options.AllowStatusCode404Response = true;
            options.ExceptionHandler = default;
        });
    }
}
