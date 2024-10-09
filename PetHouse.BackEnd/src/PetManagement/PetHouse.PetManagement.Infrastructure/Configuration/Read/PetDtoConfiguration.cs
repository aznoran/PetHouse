using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.PetManagement.Infrastructure.Configuration.Read;

public partial class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");

        builder.ComplexProperty(p => p.PetIdentifier, pi =>
        {
            pi.Property(pt => pt.BreedId).HasColumnName("breed_id");
            pi.Property(pt => pt.SpeciesId).HasColumnName("species_id");
        });
        
        builder.Property(p => p.Requisites)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<RequisiteDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("requisites");

        builder.Property(p => p.PetPhotos)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<PetPhotoDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("pet_photos");

        builder.HasKey(p => p.Id);
    }
}