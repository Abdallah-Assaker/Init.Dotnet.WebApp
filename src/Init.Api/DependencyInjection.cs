namespace Init.Api;

public static class DependencyInjection
{
    internal static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddProblemDetails();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCorsPolicy();
        
        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return services;
    }
}