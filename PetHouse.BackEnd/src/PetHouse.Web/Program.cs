using PetHouse.Accounts.Application;
using PetHouse.Accounts.Infrastructure;
using PetHouse.Accounts.Infrastructure.Seeding;
using PetHouse.Accounts.Presentation;
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

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

LoggerInitializer.ConfigureLogger(builder.Configuration);

builder.Services
    .AddAccountsApplication()
    .AddAccountsInfrastructure(builder.Configuration)
    .AddAccountsPresentation()
    .AddPetManagementPresentation()
    .AddPetManagementApplication()
    .AddPetManagementInfrastructure(builder.Configuration)
    .AddSpecieManagementPresentation()
    .AddSpecieManagementApplication()
    .AddSpecieManagementInfrastructure(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AdminAccountsSeeder>();

await accountsSeeder.SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionLogMiddleware();

app.UseAuthentication();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();