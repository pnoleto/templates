using Application.Mediator.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Linq;

namespace Application.Mediator
{
    public static class MediatorExtension
    {
        public static bool IsEventHandlerConcretClass(this Type type)
        {
            return type.Name == typeof(IEventHandler<>).Name;
        }

        public static bool IsEventHandlerWithResultConcretClass(this Type type)
        {
            return type.Name == typeof(IEventHandler<,>).Name;
        }

        public static IServiceCollection LoadAllMediatorWorkflows(this IServiceCollection services, Assembly assembly)
        {
            IEnumerable<Type> types = assembly.GetTypes();

            foreach (TypeInfo type in types.Cast<TypeInfo>())
            {
                if (!Enumerable.Any(type.ImplementedInterfaces)) continue;

                if (!IsEventHandler(type)) continue;

                Type serviceType = type.ImplementedInterfaces.First();

                services.AddSingleton(serviceType, type.AsType());
            }

            return services.AddSingleton<IMediator>(new Mediator(services));
        }

        private static bool IsEventHandler(TypeInfo handlerType)
        {
            Type type = handlerType.ImplementedInterfaces.First();

            return type.IsEventHandlerConcretClass() || type.IsEventHandlerWithResultConcretClass();
        }
    }

    public class Mediator : IMediator
    {
        private readonly Dictionary<Type, IEventHandlerBase> _workflows = [];
        private readonly IServiceCollection _services;

        public Mediator(IServiceCollection services)
        {
            _services = services;

            LoadAllHandlers();
        }

        private static bool IsEventHandler(ServiceDescriptor x)
        {
            return x.ServiceType.Name == typeof(IEventHandler<,>).Name || x.ServiceType.Name == typeof(IEventHandler<>).Name;
        }

        private void LoadAllHandlers()
        {
            ServiceProvider provider = _services.BuildServiceProvider();


            foreach ((ServiceDescriptor service, Type eventType) in from ServiceDescriptor service in _services.Where(x => IsEventHandler(x))
                                                 let eventType = service.ServiceType.GenericTypeArguments[0]
                                                 select (service, eventType))
            {
                _workflows[eventType] = (IEventHandlerBase)provider.GetRequiredService(service.ServiceType);
            }
        }

        public Task Send<TEvent>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent
        {
            var srv = _workflows[request.GetType()];

            return ((IEventHandler<TEvent>)srv).Execute(request, cancellationToken);
        }

        public Task<TResponse> Send<TEvent, TResponse>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent<TResponse>
        {
            var srv = _workflows[request.GetType()];

            return ((IEventHandler<TEvent, TResponse>)srv).Execute(request, cancellationToken);
        }
    }

}
