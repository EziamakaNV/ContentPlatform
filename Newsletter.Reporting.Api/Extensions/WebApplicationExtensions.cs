using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Database;
using Newsletter.Reporting.Middleware;

namespace Newsletter.Reporting.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ApplyMigrations(this WebApplication app) {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }

    
}
