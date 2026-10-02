using Autofac;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application;
using Portfolio.Infrastructure.Persistence.Repositories;

namespace Portfolio.Infrastructure.Persistence;

public sealed class PersistenceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ApplicationDbContext>()
            .As<DbContext>()
            .InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(Repository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();
    }
}