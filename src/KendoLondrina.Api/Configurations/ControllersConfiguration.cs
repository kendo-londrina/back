using KenLo.Api.Filters;
using Microsoft.OpenApi.Models;

namespace KenLo.Api.Configurations;

public static class ControllersConfiguration
{
    public static IServiceCollection AddAndConfigureControllers(
        this IServiceCollection services
    )
    {
        services
            .AddControllers(options 
                => options.Filters.Add(typeof(ApiGlobalExceptionFilter))
            );
            // .AddJsonOptions(jsonOptions => {
            //     jsonOptions.JsonSerializerOptions.PropertyNamingPolicy
            //         = new JsonSnakeCasePolicy();
            // });
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });
        services.AddDocumentation();
        return services;
    }

    private static IServiceCollection AddDocumentation(
        this IServiceCollection services
    )
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Kendo Londrrina", Version = "v1" });
        });
        return services;
    }

    public static WebApplication UseDocumentation(
        this WebApplication app
    ){
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        return app;
    }
}