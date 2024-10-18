using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.Constraints;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Infrastructure.Data;

public class AccountsDbContext : IdentityDbContext<User, Role, Guid>
{
    private readonly IConfiguration _configuration;
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<AdminAccount> AdminAccounts { get; set; }
    public DbSet<ParticipantAccount> ParticipantAccounts { get; set; }

    public AccountsDbContext([FromServices]IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("accounts");
        
        modelBuilder.Entity<User>().ToTable("users");
        
        modelBuilder.Entity<Role>().ToTable("roles");
        
        modelBuilder.Entity<Permission>().ToTable("permissions");
        
        modelBuilder.Entity<ParticipantAccount>().ToTable("participant_accounts");

        modelBuilder.Entity<VolunteerAccount>().ToTable("volunteer_accounts");
        
        modelBuilder.Entity<AdminAccount>().ToTable("admin_accounts");
        
        modelBuilder.Entity<User>()
            .Property(u => u.SocialNetworks)
            .HasValueObjectsJsonConversion(
                input => new SocialNetworksDto(input.Name, input.Link),
                output => SocialNetwork.Create(output.Name, output.Link).Value)
            .HasColumnName("social_networks");
        
        modelBuilder.Entity<VolunteerAccount>()
            .Property(u => u.Requisites)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto(input.Name, input.Description),
                output => Requisite.Create(output.Name, output.Description).Value)
            .HasColumnName("requisites");
        
        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");

        modelBuilder.Entity<Permission>().HasMany(p => p.Roles)
            .WithMany()
            .UsingEntity<RolePermission>();
        
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });
        
        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();

        modelBuilder.Entity<ParticipantAccount>().ComplexProperty(a => a.FullName, ab =>
        {
            ab.Property(abt => abt.Name).HasColumnName("name");
            ab.Property(abt => abt.Surname).HasColumnName("surname");
        });
        
        modelBuilder.Entity<VolunteerAccount>().ComplexProperty(a => a.FullName, ab =>
        {
            ab.Property(abt => abt.Name).HasColumnName("name");
            ab.Property(abt => abt.Surname).HasColumnName("surname");
        });
        
        modelBuilder.Entity<AdminAccount>().ComplexProperty(ac => ac.FullName, acb =>
        {
            acb.Property(fn => fn.Name).HasColumnName("name");
            acb.Property(fn => fn.Surname).HasColumnName("surname");
        });

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(DefaultConstraints.DATABASE),
            x => x.MigrationsHistoryTable("__MyMigrationsHistory", "public"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        
    }
}