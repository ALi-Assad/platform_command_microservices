using System;
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

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

        if (!appDbContext.Platforms.Any())
        {
            Console.WriteLine("Seeding data...");

            appDbContext.Platforms.AddRange(

             new Platform()
             {
                 Name = "Asp.net",
                 Publisher = "Microsoft",
                 Cost = "Free"
             },
              new Platform()
              {
                  Name = "Sql Server",
                  Publisher = "Microsoft",
                  Cost = "Free"
              },
              new Platform()
              {
                  Name = "Kubernates",
                  Publisher = "Cloud Native Computing Network",
                  Cost = "Free"
              }

            );

            appDbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("We already have data");
        }
    }
}
