using Application;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class MediatorRegister
    {
        public static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SourceEvent>());
    }
}
