using System;
using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
      public CommandsProfile(){
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommadReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
              .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
