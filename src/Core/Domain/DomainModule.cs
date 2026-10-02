using Autofac;

namespace Portfolio.Domain;

public sealed class DomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(t => t.Name.EndsWith("Factory") || t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}