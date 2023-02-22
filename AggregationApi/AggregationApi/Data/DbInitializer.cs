using Microsoft.EntityFrameworkCore;

namespace AggregationApi.Data
{
    public static class DbInitializer
    {
        public static void ApplyMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
