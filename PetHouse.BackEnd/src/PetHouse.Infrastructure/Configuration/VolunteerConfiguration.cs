using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Domain;
using PetHouse.Domain.Constraints;

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
        
        builder.Property(p => p.FullName).IsRequired().HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH);
        builder.Property(p => p.YearsOfExperience).IsRequired();
        builder.Property(p => p.CountOfPetsFoundHome).IsRequired();
        builder.Property(p => p.CountOfPetsLookingForHome).IsRequired();
        builder.Property(p => p.CountOfPetsOnTreatment).IsRequired();
        builder.Property(p => p.PhoneNumber).IsRequired();

        builder.HasMany(v => v.Pets)
            .WithOne();

        builder.OwnsOne(v => v.Requisites, r =>
        {
            r.ToJson();
            r.OwnsMany(ri => ri.Requisites);
        });
        
        builder.OwnsOne(v => v.SocialNetworks, sn =>
        {
            sn.ToJson();
            sn.OwnsMany(sni => sni.SocialNetworks);
        });
        
    }
}