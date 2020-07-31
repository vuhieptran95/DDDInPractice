using System.Reflection;
using Autofac;
using DDDInPractice.Persistence.Features;
using DDDInPractice.Persistence.Infrastructure.Requests.Authorizations;
using DDDInPractice.Persistence.Infrastructure.Requests.Caching;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.Logging;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestEvents;
using DDDInPractice.Persistence.Infrastructure.Requests.Validations;
using Microsoft.Extensions.Caching.Memory;
using ResponsibilityChain;
using Module = Autofac.Module;

namespace DDDInPractice.Persistence.Infrastructure
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterType<Mediator>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<MemoryCache>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterGeneric(typeof(DefaultHandler<,>)).InstancePerLifetimeScope();
            
            builder.RegisterGeneric(typeof(RequestHandler<,>)).InstancePerLifetimeScope();
            
            
            builder.RegisterGeneric(typeof(AuthorizationConfigBase<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(AuthorizationHandler<,>)).InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(IPreAuthorizationRule<,>))
                .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(IPostAuthorizationRule<,>))
                .InstancePerLifetimeScope();
            
            builder.RegisterGeneric(typeof(DefaultHandler<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ValidationHandlerBase<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ExecutionHandlerBase<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(LoggingHandler<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RequestEventHandler<,>)).InstancePerLifetimeScope();
            
            builder.RegisterGeneric(typeof(CacheHandler<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(CacheConfig<>))
                .As(typeof(ICacheConfig<>))
                .InstancePerLifetimeScope();
            
            builder.RegisterGeneric(typeof(DefaultAuthorizationConfig<>))
                .As(typeof(IAuthorizationConfig<>))
                .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(ICacheConfig<>))
                .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(IAuthorizationConfig<>))
                .InstancePerLifetimeScope();
                
            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(IPreValidation<,>))
                .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(IPostValidation<,>))
                .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(IExecution<,>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(Handler<,>))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AsClosedTypesOf(typeof(IRequest<>));
            
            base.Load(builder);
        }
    }
}