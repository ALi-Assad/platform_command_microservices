using System;

namespace CommandService.Dtos;

public class CommadReadDto
{
    public required int Id { get; set; }
    public required string HowTo { get; set; }
    public required string CommandLine { get; set; }
    public required int PlatformId { get; set; }

}
