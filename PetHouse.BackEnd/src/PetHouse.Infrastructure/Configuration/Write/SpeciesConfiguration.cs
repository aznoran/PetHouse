using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Specie.Aggregate;
using PetHouse.Domain.Specie.Entities;

namespace PetHouse.Infrastructure.Configuration.Write;

public partial class SpeciesConfiguration : IEntityTypeConfiguration<Specie>
{
    public void Configure(EntityTypeBuilder<Specie> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value)
            );

        builder.ComplexProperty(s => s.Name, sn =>
        {
            sn.Property(n => n.Value).HasColumnName("name");
        });
        
        builder.HasMany(v => v.Breeds)
            .WithOne(b => b.Specie)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        /*builder.OwnsMany(s => s.Breeds, sp =>
        {
            sp.ToTable("breeds");
            
            sp.Property(bt => bt.Id)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value)
                );
        });*/
    }
}