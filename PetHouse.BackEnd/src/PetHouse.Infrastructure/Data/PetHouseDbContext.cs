using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.Domain;
using PetHouse.Domain.Constraints;


namespace PetHouse.Infrastructure;

public class PetHouseDbContext(IConfiguration configuration) : DbContext()
{
    ILoggerFactory loggerFactory = new LoggerFactory();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DefaultConstraints.DATABASE));
        //EFCore.NamingConventions
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}