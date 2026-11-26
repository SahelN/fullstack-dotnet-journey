using MovieApi.Data;
using MovieApi.Data.Seed;

namespace MovieApi.Extensions;

public static class ApplicationExtensions
{
    public static async Task SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<MovieContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            await DbSeeder.SeedAsync(context);
            logger.LogInformation("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}