using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SpecieManagement.Domain.Aggregate;

namespace PetHouse.SpecieManagement.Infrastructure.Data;

public class WriteDbContext(IConfiguration configuration) : DbContext()
{
    readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    public DbSet<Specie> Species { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly, type =>
            type.FullName?.Contains("Configuration.Write") ?? false);
        
        modelBuilder.HasDefaultSchema("species");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DefaultConstraints.DATABASE),
            x => x.MigrationsHistoryTable("__MyMigrationsHistory", "public"));

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}