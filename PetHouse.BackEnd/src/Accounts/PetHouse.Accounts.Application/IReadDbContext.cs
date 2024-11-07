using Microsoft.EntityFrameworkCore;
using PetHouse.Accounts.Contracts.Dtos;

namespace PetHouse.Accounts.Application;

public interface IReadDbContext
{
    public DbSet<UserDto> Users { get; set; }
    
    public DbSet<RoleDto> Roles { get; set; }
    
    public DbSet<AdminAccountDto> AdminAccounts { get; set; }
    
    public DbSet<ParticipantAccountDto> ParticipantAccounts { get; set; }
    
    public DbSet<VolunteerAccountDto> VolunteerAccounts { get; set; }
}