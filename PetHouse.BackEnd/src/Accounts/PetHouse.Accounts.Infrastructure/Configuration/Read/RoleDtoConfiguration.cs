using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Accounts.Contracts.Dtos;

namespace PetHouse.Accounts.Infrastructure.Configuration.Read;

public class RoleDtoConfiguration : IEntityTypeConfiguration<RoleDto>
{
    public void Configure(EntityTypeBuilder<RoleDto> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id);
    }
}