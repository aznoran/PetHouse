using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Accounts.Contracts.Dtos;

namespace PetHouse.Accounts.Infrastructure.Configuration.Read;

public class UserRolesDtoConfiguration : IEntityTypeConfiguration<UserRolesDto>
{
    public void Configure(EntityTypeBuilder<UserRolesDto> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(u => new { u.UserId, u.RoleId });
    }
}