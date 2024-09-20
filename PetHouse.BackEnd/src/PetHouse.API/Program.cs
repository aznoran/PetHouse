using Microsoft.EntityFrameworkCore;
using PetHouse.API;
using PetHouse.API.Extensions;
using PetHouse.API.Logging;
using PetHouse.Application;
using PetHouse.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LoggerInitializer.ConfigureLogger(builder.Configuration);

builder.Services
    .AddApiServices()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await using var scope = app.Services.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<PetHouseDbContext>();
    
    await dbContext.Database.MigrateAsync();
}

app.UseExceptionLogMiddleware();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();