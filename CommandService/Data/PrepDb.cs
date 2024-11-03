using System;
using Microsoft.EntityFrameworkCore;
using CommandService.Models;

namespace CommandService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app, bool IsProduction)
    {
        using var servicesScope = app.ApplicationServices.CreateScope();
        var appDbContext = servicesScope.ServiceProvider.GetService<AppDbContext>();
        ArgumentNullException.ThrowIfNull(appDbContext);
        SeedData(appDbContext, IsProduction);
    }

    private static void SeedData(AppDbContext appDbContext, bool IsProduction)
    {
        if (IsProduction)
        {
            try
            {
                Console.WriteLine($"Attempting to apply migration");
                appDbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faild to run migration error: {ex.Message}");
            }
        }
    }
}
