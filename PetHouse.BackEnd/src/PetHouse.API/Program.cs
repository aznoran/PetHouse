using Microsoft.EntityFrameworkCore;
using PetHouse.Infrastructure;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PetHouseDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PetHouseDbContext"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();