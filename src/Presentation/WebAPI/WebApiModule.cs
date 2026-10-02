using Autofac;

namespace Portfolio.Presentation.WebAPI;

public sealed class WebApiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = ThisAssembly;

        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("Endpoint") || t.Name.EndsWith("Controller"))
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}