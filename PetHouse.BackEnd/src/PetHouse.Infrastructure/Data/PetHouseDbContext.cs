using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace PetHouse.Infrastructure;

public class PetHouseDbContext : DbContext
{
    
    ILoggerFactory loggerFactory = new LoggerFactory();
    
    public PetHouseDbContext(DbContextOptions<PetHouseDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            
        //EFCore.NamingConventions
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}