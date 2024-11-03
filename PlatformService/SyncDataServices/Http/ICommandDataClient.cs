using System;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public interface ICommandDataClient
{

    public Task SentPlatformToCommand(PlatformReadDto platformReadDto);

}
