using System;
using System.Text;
using System.Text.Json;
using Azure;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration) : ICommandDataClient
{
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;
    public async Task SentPlatformToCommand(PlatformReadDto platformReadDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platformReadDto),
            Encoding.UTF8,
            "application/json"
        ); 

        var response = await _httpClient.PostAsJsonAsync($"{_configuration["CommandServiceUrl"]}", httpContent);

        if(response.IsSuccessStatusCode){
            Console.WriteLine("-- platform sent to command service successfuly --");
        }else{
            Console.WriteLine("-- platform didn't sent to command service successfuly --");
        }
    }
}
