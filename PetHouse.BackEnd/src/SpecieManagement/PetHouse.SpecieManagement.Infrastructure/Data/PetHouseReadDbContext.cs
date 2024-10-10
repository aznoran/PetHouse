using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.Core.Dtos.SpeciesManagment;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SpecieManagement.Application;

namespace PetHouse.SpecieManagement.Infrastructure.Data;

public class PetHouseReadDbContext(IConfiguration configuration) : DbContext(), IReadDbContext
{
    readonly ILoggerFactory _loggerFactory = new LoggerFactory();

    public DbSet<SpecieDto> Species => Set<SpecieDto>();
    public DbSet<BreedDto> Breeds => Set<BreedDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseReadDbContext).Assembly, type =>
                type.FullName?.Contains("Configuration.Read") ?? false);

            modelBuilder.Entity<VolunteerDto>().HasQueryFilter(v => !v.IsDeleted);
            
            modelBuilder.Entity<PetDto>().HasQueryFilter(v => !v.IsDeleted);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);

        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DefaultConstraints.DATABASE));

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}