using System;
using CommandService.Data;
using CommandService.Models;

namespace PlatformService.Data.Repository;

public class CommandRepo(AppDbContext appDbContext) : ICommandRepo
{
    public void CreateCommand(int platformId, Command command)
    {
         ArgumentNullException.ThrowIfNull(command);
         command.PlatformId = platformId;

         appDbContext.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        appDbContext.Platforms.Add(platform);
    }

    public bool ExternalPlatformExists(int ExterbalPlatformId)
    {
         return appDbContext.Platforms.Any(p => p.ExternalID == ExterbalPlatformId);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
       return appDbContext.Platforms.ToList();
    }

    public Command? GetCommand(int platformId, int CommandId)
    {
       return appDbContext.Commands.FirstOrDefault(c => c.Id == CommandId && c.PlatformId == platformId);
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return appDbContext.Commands
               .Where(c => c.PlatformId == platformId)
               .OrderBy(c => c.Platform != null ? c.Platform.Name : c.HowTo);
    }

    public bool PlatformExists(int platformId)
    {
        return appDbContext.Platforms.Any(p => p.Id == platformId);
    }

    public bool SaveChanges()
    { 
        return appDbContext.SaveChanges() >= 0;
    }
}
