using PetHouse.PetManagement.Application;
using PetHouse.PetManagement.Infrastructure;
using PetHouse.PetManagement.Presentation;
using PetHouse.SpecieManagement.Application;
using PetHouse.SpecieManagement.Infrastructure;
using PetHouse.SpecieManagement.Presentation;
using PetHouse.Web;
using PetHouse.Web.Extensions;
using PetHouse.Web.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LoggerInitializer.ConfigureLogger(builder.Configuration);

builder.Services
    .AddApiServices(builder.Configuration)
    .AddPetManagementPresentation()
    .AddPetManagementApplication()
    .AddPetManagementInfrastructure(builder.Configuration)
    .AddSpecieManagementPresentation()
    .AddSpecieManagementApplication()
    .AddSpecieManagementInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionLogMiddleware();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();