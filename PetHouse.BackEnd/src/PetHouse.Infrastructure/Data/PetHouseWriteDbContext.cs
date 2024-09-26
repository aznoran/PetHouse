using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.Domain.PetManagment.Aggregate;
using PetHouse.Domain.Specie.Entities;

namespace PetHouse.Infrastructure.Data;

public class PetHouseWriteDbContext(IConfiguration configuration) : DbContext()
{
    readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    
    public DbSet<Volunteer> Volunteers { get; set; }

    public DbSet<Species> Species { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseWriteDbContext).Assembly, type =>
            type.FullName?.Contains("Configuration.Write") ?? false);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.Constants.DATABASE));

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}