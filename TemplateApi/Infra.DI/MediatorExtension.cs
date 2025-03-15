using Application.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class MediatorExtension
    {
        public static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddMediatR(config => config
            .RegisterServicesFromAssemblyContaining(typeof(EverythingHandler)));
    }
}
