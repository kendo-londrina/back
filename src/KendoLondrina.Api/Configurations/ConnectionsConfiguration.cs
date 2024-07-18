using KenLo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace KenLo.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConections(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbConnection(configuration);
        return services;
    }

    private static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<KendoLondrinaDbContext>(
            options => options.UseInMemoryDatabase(
                "end2end-test-db" // "inMemory-DSV-Databases"
            )
        );
        return services;
    }

    // public static WebApplication MigrateDatabase(
    //     this WebApplication app)
    // {
    //     var environment = Environment
    //         .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    //     if (environment == "EndToEndTest") return app;
    //     using var scope = app.Services.CreateScope();
    //     var dbContext = scope.ServiceProvider
    //         .GetRequiredService<CodeflixCatalogDbContext>();
    //     dbContext.Database.Migrate();
    //     return app;
    // }
}