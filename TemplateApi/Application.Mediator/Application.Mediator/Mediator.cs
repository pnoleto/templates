using System.Reflection;
using Application.Mediator.Interface;
using Microsoft.Extensions.DependencyInjection;

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

        public static bool IsEventHandler(TypeInfo handlerType)
        {
            Type type = handlerType.ImplementedInterfaces.First();

            return type.IsEventHandlerConcretClass() || type.IsEventHandlerWithResultConcretClass();
        }

        public static bool IsEventHandler(this ServiceDescriptor x)
        {
            return x.ServiceType.Name == typeof(IEventHandler<,>).Name || x.ServiceType.Name == typeof(IEventHandler<>).Name;
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

        private IEnumerable<(ServiceDescriptor service, Type eventType)> LoadAllServiceInstances()
        {          
            return from ServiceDescriptor service in _services.Where(svc => svc.IsEventHandler())
                   let eventType = service.ServiceType.GenericTypeArguments[0]
                   select (service, eventType);
        }

        private void LoadAllHandlers()
        {
            using IServiceScope scope = _services.BuildServiceProvider().CreateScope();

            foreach ((ServiceDescriptor service, Type eventType) in LoadAllServiceInstances())
                _workflows[eventType] = (IEventHandlerBase)scope.ServiceProvider.GetRequiredService(service.ServiceType);
        }

        public ValueTask Send<TEvent>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent
        {
            IEventHandlerBase handlerInstance = _workflows[request.GetType()];

            return ((IEventHandler<TEvent>)handlerInstance).Execute(request, cancellationToken);
        }

        public ValueTask<TResponse> Send<TEvent, TResponse>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent<TResponse>
        {
            IEventHandlerBase handlerInstance = _workflows[request.GetType()];

            return ((IEventHandler<TEvent, TResponse>)handlerInstance).Execute(request, cancellationToken);
        }
    }

}
