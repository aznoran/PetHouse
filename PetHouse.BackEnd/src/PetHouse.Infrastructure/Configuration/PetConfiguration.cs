using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Domain;
using PetHouse.Domain.Constraints;

namespace PetHouse.Infrastructure.Configuration;

public partial class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
            id => id.Value,
            value => PetId.Create(value)
                );

        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH);
        builder.Property(p => p.Address).IsRequired();
        builder.Property(p => p.PhoneNumber).IsRequired();
        builder.Property(p => p.PetStatus).IsRequired();
        builder.Property(p => p.CreationDate).IsRequired();

        builder.OwnsOne(p => p.Requisites, r =>
        {
            r.ToJson();
            r.OwnsMany(ri => ri.Requisites);
        });
        
        builder.OwnsOne(p => p.PetPhotos, pp =>
        {
            pp.ToJson();
            pp.OwnsMany(ppi => ppi.PetPhotos);
        });

        builder.OwnsOne(p => p.PetIdentifier, pi =>
        {
            pi.Property(pit => pit.SpeciesId).IsRequired().HasColumnName("species_id");
            pi.Property(pit => pit.BreedId).IsRequired().HasColumnName("breed_id");
        });
    }
}