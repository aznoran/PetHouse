using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SpecieManagement.Domain.Aggregate;

namespace PetHouse.SpecieManagement.Infrastructure.Data;

public class PetHouseWriteDbContext(IConfiguration configuration) : DbContext()
{
    readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    public DbSet<Specie> Species { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseWriteDbContext).Assembly, type =>
            type.FullName?.Contains("Configuration.Write") ?? false);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DefaultConstraints.DATABASE));

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}