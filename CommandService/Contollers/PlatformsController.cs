using System;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Repository;

namespace CommandService.Contollers;

[ApiController]
[Route("api/c/Platform")]
public class PlatformsController(ICommandRepo commandRepo, IMapper mapper) : ControllerBase
{
      private readonly IMapper _mapper = mapper;
      private readonly ICommandRepo _commandRepo = commandRepo;

      [HttpGet]
      public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
      {
              Console.WriteLine("Geting platforms from command");

              var platforms = _commandRepo.GetAllPlatforms();
              Console.WriteLine(platforms);
              return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
      }
      
       [HttpPost]
      public IActionResult TestInboundConnection(PlatformReadDto platformReadDto)
      {
         Console.WriteLine("Test Inbound");

         return Ok("in bound");
      }
}