using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class MediatorRegister
    {
        public static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(MediatorRegister)));
    }
}
