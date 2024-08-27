using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Domain;
using PetHouse.Domain.Constraints;
using PetHouse.Domain.Models;

namespace PetHouse.Infrastructure.Configuration;

public partial class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value)
            );
        
        builder.ComplexProperty(v => v.VolunteerProfile, vp =>
        {
            vp.Property(p => p.FullName).IsRequired().HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH).HasColumnName("full_name");
            vp.Property(p => p.Description).IsRequired().HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH).HasColumnName("description");
            vp.Property(p => p.YearsOfExperience).IsRequired().HasColumnName("years_of_experience");
            vp.Property(p => p.PhoneNumber).IsRequired().HasColumnName("phone_number");
        });

        builder.HasMany(v => v.Pets)
            .WithOne();

        builder.OwnsOne(v => v.Requisites, r =>
        {
            r.ToJson("requisites");
            r.OwnsMany(ri => ri.Requisites, rt =>
            {
                rt.Property(t => t.Description).HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH).HasJsonPropertyName("description");
                rt.Property(t => t.Name).HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH).HasJsonPropertyName("name");
            });
        });
        
        builder.OwnsOne(v => v.SocialNetworks, sn =>
        {
            sn.ToJson("social_networks");
            sn.OwnsMany(sni => sni.SocialNetworks, snt =>
            {
                snt.Property(t => t.Link).HasMaxLength(DefaultConstraints.MAX_LINK_LENGTH).HasJsonPropertyName("reference");
                snt.Property(t => t.Name).HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH).HasJsonPropertyName("name");
            });
        });
        
    }
}