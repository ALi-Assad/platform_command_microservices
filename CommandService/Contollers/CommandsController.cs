using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Contollers
{
    [Route("api/c/Platform/{platformId}/Commands")]
    [ApiController]
    public class CommandsController(ICommandRepo commandRepo, IMapper mapper) : ControllerBase
    {
        private readonly ICommandRepo _commandRepo = commandRepo;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public ActionResult<IEnumerable<CommadReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"Geting commands for platfom with id {platformId}");
            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _commandRepo.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommadReadDto>>(commands));

        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommadReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"Geting command with id {commandId} for platfom with id {platformId}");
            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _commandRepo.GetCommand(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommadReadDto>(command));

        }

        [HttpPost()]
        public ActionResult<CommadReadDto> CreateCommand(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"Creating Command");

            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }


            var command = _mapper.Map<Command>(commandDto);
            _commandRepo.CreateCommand(platformId, command);
            _commandRepo.SaveChanges();

            return CreatedAtRoute(nameof(GetCommandForPlatform),
             new { platformId, commandId = command.Id },
              _mapper.Map<CommadReadDto>(command));
        }
    }
}
