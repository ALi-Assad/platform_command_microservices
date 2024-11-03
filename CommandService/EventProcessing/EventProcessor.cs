using System;
using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing;

public class EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper) : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly IMapper _mapper = mapper;

    public void ProcessEvent(string message)
    {
        var eventType = _DetermineEvent(message);

        switch (eventType)
        {
            case EventTypeEnum.PlatformPublished:
                _AddPlatform(message);
                break;
            default:
                break;
        }
    }

    private EventTypeEnum _DetermineEvent(string notifcationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

        switch (eventType?.Event)
        {
            case "platform_published":
                Console.WriteLine("--> Platform Published Event Detected");
                return EventTypeEnum.PlatformPublished;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventTypeEnum.Undetermined;
        }
    }

    private void _AddPlatform(string platformPublishedMessage)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
            var plat = _mapper.Map<Platform>(platformPublishedDto);
            Console.WriteLine($"---> mapped platform id to add in command DB {plat.Id}");
            if (!repo.ExternalPlatformExists(plat.ExternalID))
            {
                repo.CreatePlatform(plat);
                repo.SaveChanges();
                Console.WriteLine("--> Platform added!");
            }
            else
            {
                Console.WriteLine("--> Platform already exisits...");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
        }
    }
}