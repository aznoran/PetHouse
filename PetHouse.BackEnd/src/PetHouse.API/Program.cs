using PetHouse.API;
using PetHouse.API.Extensions;
using PetHouse.Application;
using PetHouse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<PetHouseDbContext>(_ => new PetHouseDbContext(builder.Configuration));

builder.Services
    .AddApiServices()
    .AddInfrastructure()
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionLogMiddleware();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();