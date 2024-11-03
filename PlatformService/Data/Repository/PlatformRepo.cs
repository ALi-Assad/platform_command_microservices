using System;
using PlatformService.Models;

namespace PlatformService.Data.Repository;

public class PlatformRepo(AppDbContext appDbContext) : IPlatformRepo
{
    public void Create(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);

        appDbContext.Platforms.Add(platform);
    }

    public Platform? Get(int id)
    {
        return appDbContext.Platforms.FirstOrDefault<Platform>(p => p.Id == id);
    }

    public IEnumerable<Platform> GetAll()
    {
       return appDbContext.Platforms.ToList();
    }

    public bool SaveChanges()
    {
        return appDbContext.SaveChanges() >= 0;
    }
}
