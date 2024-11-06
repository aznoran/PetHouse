using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Infrastructure.Configuration.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<IdentityUserRole<Guid>>();

        builder.Property(u => u.SocialNetworks)
            .HasValueObjectsJsonConversion(
                input => new SocialNetworksDto(input.Name, input.Link),
                output => SocialNetwork.Create(output.Name, output.Link).Value)
            .HasColumnName("social_networks");
        
        builder.HasOne(u => u.AdminAccount)
            .WithOne(s => s.User)
            .HasForeignKey<AdminAccount>("user_id")
            .IsRequired(false);
        
        builder.HasOne(u => u.VolunteerAccount)
            .WithOne(s => s.User)
            .HasForeignKey<VolunteerAccount>("user_id")
            .IsRequired(false);
        
        builder.HasOne(u => u.ParticipantAccount)
            .WithOne(s => s.User)
            .HasForeignKey<ParticipantAccount>("user_id")
            .IsRequired(false);
    }
}