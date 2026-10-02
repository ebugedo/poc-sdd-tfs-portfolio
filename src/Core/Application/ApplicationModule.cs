using Autofac;
using FluentValidation;
using MediatR;

namespace Portfolio.Application;

public sealed class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = ThisAssembly;

        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
    }
}