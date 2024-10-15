using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.PetManagement.Domain.Aggregate;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.PetManagement.Infrastructure.Data;

public class PetHouseWriteDbContext(IConfiguration configuration) : DbContext()
{
    readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    
    public DbSet<Volunteer> Volunteers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseWriteDbContext).Assembly, type =>
            type.FullName?.Contains("Configuration.Write") ?? false);
        
        modelBuilder.HasDefaultSchema("volunteers");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DefaultConstraints.DATABASE),
            x => x.MigrationsHistoryTable("__MyMigrationsHistory", "public"));

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}