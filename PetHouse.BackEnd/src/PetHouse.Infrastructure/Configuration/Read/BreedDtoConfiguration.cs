using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Application.Dtos.SpeciesManagment;

namespace PetHouse.Infrastructure.Configuration.Read;

public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SpecieId).HasColumnName("specie_id");
    }
}