using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Data.Repository;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("-- In Memory DB --");
    builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseInMemoryDatabase("InMem")
    );
}
else
{
    Console.WriteLine("-- Sql Server DB --");
    builder.Services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn"))
    );
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

Console.WriteLine(builder.Configuration["CommandServiceUrl"]);
Console.WriteLine($"---> Rabbit mq port from program: {builder.Configuration["RabbitMQport"]}");

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.PrepPopulation(app.Environment.IsProduction());
app.MapControllers();
app.Run();
