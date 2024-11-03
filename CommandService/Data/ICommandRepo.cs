using System;
using CommandService.Models;

namespace CommandService.Data;

public interface ICommandRepo
{
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExists(int platformId);
    bool ExternalPlatformExists(int ExterbalPlatformId);


    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command? GetCommand(int platformId, int CommandId);
    void CreateCommand(int platformId, Command command);
    
    bool SaveChanges();
    
}
