using Init.Api;
using Init.Application;
using Init.Infrastructure;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    {
        // Configure Serilog at the host level
        builder.Host.UseSerilog();
        
        builder.Services
            .AddPresentation()
            .AddApplication()
            .AddInfrastructure(builder.Configuration);
    }

    var app = builder.Build();
    {
        // if (app.Environment.IsDevelopment())
        // {
            app.UseSwagger();
            app.UseSwaggerUI();
        // }
        
        app.UseCors("AllowAllOrigins");

        app.UseHttpsRedirection();
        
        app.UseHangfireJobsKitMiddleware(app.Configuration);
        
        app.RegisterRecurringJobs();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        
        app.Run();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}