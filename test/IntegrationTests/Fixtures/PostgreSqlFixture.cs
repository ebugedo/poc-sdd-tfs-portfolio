using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Portfolio.Infrastructure.Persistence;

namespace Portfolio.IntegrationTests.Fixtures;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    public string ConnectionString { get; private set; } = string.Empty;
    internal PostgreSqlContainer Container => _container;

    public PostgreSqlFixture()
    {
        _container = new PostgreSqlBuilder()
            .WithDatabase("portfolio_test")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        ConnectionString = _container.GetConnectionString();

        // Run migrations to set up the database schema
        await RunMigrationsAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    private async Task RunMigrationsAsync()
    {
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(ConnectionString));
        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
}

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlFixture _postgreSqlFixture;

    public PostgreSqlContainer Container => _postgreSqlFixture.Container;

    public CustomWebApplicationFactory()
    {
        _postgreSqlFixture = new PostgreSqlFixture();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(ApplicationDbContext));

            // Add test database context
            var connectionString = _postgreSqlFixture.ConnectionString;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Ensure database is created and migrated
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        });
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlFixture.InitializeAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlFixture.DisposeAsync();
        base.Dispose();
    }
}

[CollectionDefinition("PostgreSqlCollection")]
public class PostgreSqlCollection : ICollectionFixture<PostgreSqlFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}