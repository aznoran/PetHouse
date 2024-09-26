using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Application.Dtos.SpeciesManagment;
using PetHouse.Application.Volunteers;

namespace PetHouse.Infrastructure.Data;

public class PetHouseReadDbContext(IConfiguration configuration) : DbContext(), IReadDbContext
{
    readonly ILoggerFactory _loggerFactory = new LoggerFactory();

    public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();

    //public DbSet<SpeciesDto> Species => Set<SpeciesDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseReadDbContext).Assembly, type =>
                type.FullName?.Contains("Configuration.Read") ?? false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);

        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.Constants.DATABASE));

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}