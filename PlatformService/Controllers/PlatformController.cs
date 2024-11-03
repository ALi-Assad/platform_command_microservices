using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/Platform")]
    [ApiController]
    public class PlatformController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient) : ControllerBase
    {

        private readonly IPlatformRepo _platformRepo = platformRepo;
        private readonly IMapper _mapper = mapper;
        private readonly ICommandDataClient _commandDataClient = commandDataClient;
        private readonly IMessageBusClient _messageBusClient = messageBusClient;


        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var Platforms = _platformRepo.GetAll();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(Platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var Platform = _platformRepo.Get(id);
            return Ok(_mapper.Map<PlatformReadDto>(Platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var PlatformModel = _mapper.Map<Platform>(platformCreateDto);
            _platformRepo.Create(PlatformModel);
            _platformRepo.SaveChanges();

            PlatformReadDto platformReadDto = _mapper.Map<PlatformReadDto>(PlatformModel);

            try
            {
               await _commandDataClient.SentPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't send created platform to Command {ex.Message}");
            }

            
            try
            {
               var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                   platformPublishedDto.Event = "platform_published";
                   _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't publish platform to message bus: {ex}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { id = PlatformModel.Id }, platformCreateDto);
        }
    }
}
