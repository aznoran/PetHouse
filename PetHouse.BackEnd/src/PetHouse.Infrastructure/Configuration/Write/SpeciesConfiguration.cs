using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Specie.Entities;

namespace PetHouse.Infrastructure.Configuration.Write;

public partial class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value)
            );

        builder.OwnsMany(s => s.Breeds, sp =>
        {
            sp.Property(bt => bt.Id)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value)
                );
        });
    }
}