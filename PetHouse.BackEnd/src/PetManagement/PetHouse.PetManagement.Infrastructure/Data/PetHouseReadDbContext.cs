using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.PetManagement.Application;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.PetManagement.Infrastructure.Data;

public class PetHouseReadDbContext(IConfiguration configuration) : DbContext(), IReadDbContext
{
    readonly ILoggerFactory _loggerFactory = new LoggerFactory();

    public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public DbSet<PetDto> Pets => Set<PetDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetHouseReadDbContext).Assembly, type =>
            type.FullName?.Contains("Configuration.Read") ?? false);

        modelBuilder.HasDefaultSchema("volunteers");

        modelBuilder.Entity<VolunteerDto>().HasQueryFilter(v => !v.IsDeleted);

        modelBuilder.Entity<PetDto>().HasQueryFilter(v => !v.IsDeleted);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().LogTo(Console.WriteLine);

        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DefaultConstraints.DATABASE),
            x => x.MigrationsHistoryTable("__MyMigrationsHistory", "public"));

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}