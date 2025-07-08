using Microsoft.Extensions.DependencyInjection;
using Shared.Mediator.Base;
using Shared.Mediator.Interface;
using System.Reflection;

namespace Shared.Mediator
{
    public static class MediatorExtension
    {
        public static bool IsEventHandlerConcretClass(this Type type)
        {
            return type.Name == typeof(EventHandlerBase<>).Name;
        }

        public static bool IsEventHandlerWithResultConcretClass(this Type type)
        {
            return type.Name == typeof(EventHandlerBase<,>).Name;
        }

        public static bool IsEventHandlerBased(this ServiceDescriptor type)
        {
            return type.ServiceType.Name == typeof(IEventHandler<>).Name || type.ServiceType.Name == typeof(IEventHandler<,>).Name;
        }

        public static IServiceCollection LoadAllMediatorWorkflows(this IServiceCollection services, Assembly assembly)
        {
            IEnumerable<Type> types = assembly.GetTypes();

            foreach (TypeInfo type in types.Cast<TypeInfo>())
            {
                Type implementationType = type.AsType();

                if (implementationType.BaseType is not null)
                {
                    if (implementationType.BaseType.IsEventHandlerConcretClass())
                    {
                        Type? serviceType = implementationType.GetInterface(typeof(IEventHandler<>).Name);

                        services.AddSingleton(serviceType, implementationType);
                    }

                    if (implementationType.BaseType.IsEventHandlerWithResultConcretClass())
                    {
                        Type? serviceType = implementationType.GetInterface(typeof(IEventHandler<,>).Name);

                        services.AddSingleton(serviceType, implementationType);
                    }
                }
            }

            return services.AddSingleton<IMediator>(new Mediator(services));
        }
    }


    public class Mediator : IMediator
    {
        private readonly Dictionary<Type, IEventHandlerBase> _workflows = [];

        public Mediator(IServiceCollection services)
        {
            LoadAllHandlers(services);
        }

        private void LoadAllHandlers(IServiceCollection services)
        {
            ServiceProvider provider = services.BuildServiceProvider();

            IEnumerable<ServiceDescriptor> filteredServices = services.Where(x => x.IsEventHandlerBased());

            foreach (ServiceDescriptor service in filteredServices)
            {
                IEnumerable<TypeInfo> genericTypeArguments = ((TypeInfo)service.ServiceType).GenericTypeArguments.Cast<TypeInfo>();

                Type eventType = genericTypeArguments.First().AsType();

                _workflows[eventType] = (IEventHandlerBase)provider.GetRequiredService(service.ServiceType);
            }
        }

        public Task Send<TEvent>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent
        {
            Type eventType = request.GetType();

            return ((IEventHandler<TEvent>)_workflows[eventType]).Execute(request, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IEvent<TResponse> request, CancellationToken cancellationToken) where TResponse : class
        {
            Type eventType = request.GetType();

            return ((IEventHandler<IEvent<TResponse>, TResponse>)_workflows[eventType]).Execute(request, cancellationToken);
        }
    }
}
