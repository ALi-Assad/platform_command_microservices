using System;

namespace CommandService.Dtos;

public class PlatformPublishedDto
{
     public int Id { get; set; }
     public required string Name { get; set; }
     public required string Event { get; set; }
}
