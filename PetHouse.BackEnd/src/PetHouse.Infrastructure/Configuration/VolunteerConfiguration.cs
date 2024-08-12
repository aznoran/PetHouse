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
        
        builder.Property(p => p.FullName).IsRequired().HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH);
        builder.Property(p => p.YearsOfExperience).IsRequired();
        builder.Property(p => p.CountOfPetsFoundHome).IsRequired();
        builder.Property(p => p.CountOfPetsLookingForHome).IsRequired();
        builder.Property(p => p.CountOfPetsOnTreatment).IsRequired();
        builder.Property(p => p.PhoneNumber).IsRequired();

        builder.HasMany(v => v.Pets)
            .WithOne();

        builder.OwnsMany(v => v.Requisites, r =>
        {
            r.Property(rt => rt.Description).IsRequired();
            r.Property(rt => rt.Name).IsRequired();
        });

        builder.OwnsMany(v => v.SocialNetworks, sn =>
        {
            sn.Property(snt => snt.Name).IsRequired();
            sn.Property(snt => snt.Reference).IsRequired();
        });
    }
}