using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHouse.Accounts.Application;
using PetHouse.Accounts.Contracts.Dtos;
using PetHouse.SharedKernel.Constraints;

namespace PetHouse.Accounts.Infrastructure.Data;

public class AccountsReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public DbSet<UserDto> Users { get; set; }
    
    public DbSet<RoleDto> Roles { get; set; }
    
    public DbSet<AdminAccountDto> AdminAccounts { get; set; }
    
    public DbSet<ParticipantAccountDto> ParticipantAccounts { get; set; }
    
    public DbSet<VolunteerAccountDto> VolunteerAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DefaultConstraints.DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("accounts");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AccountsReadDbContext).Assembly,
            type => type.FullName?.Contains("Configuration.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}