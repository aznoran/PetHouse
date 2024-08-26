using Microsoft.EntityFrameworkCore;
using PetHouse.Application.Volunteers;
using PetHouse.Application.Volunteers.CreateVolunteer;
using PetHouse.Infrastructure;
using PetHouse.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IVolunteersRepository, VolunteersRepository>();
builder.Services.AddScoped<CreateVolunteerHandler>();

builder.Services.AddScoped<PetHouseDbContext>(provider => new PetHouseDbContext(builder.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();