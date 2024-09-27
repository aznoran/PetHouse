using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Domain.PetManagment.Entities;
using PetHouse.Domain.PetManagment.ValueObjects;
using PetHouse.Domain.Shared.Constraints;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;
using PetHouse.Infrastructure.Extensions;

namespace PetHouse.Infrastructure.Configuration.Write;

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

        builder.ComplexProperty(p => p.Position, pi =>
        {
            pi.Property(pp => pp.Value)
                .IsRequired()
                .HasColumnName("position");
        });
        
        builder.ComplexProperty(p => p.Name, pi =>
        {
            pi.Property(pp => pp.Value).IsRequired()
                .HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH)
                .HasColumnName("name");
        });
        
        builder.ComplexProperty(p => p.PetIdentifier, pi =>
        {
            pi.Property(pit => pit.SpeciesId)
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value)
                )
                .IsRequired()
                .HasColumnName("species_id");
            pi.Property(pit => pit.BreedId).IsRequired().HasColumnName("breed_id");
        });
        
        builder.ComplexProperty(p => p.Description, pi =>
        {
            pi.Property(pp => pp.Value).IsRequired()
                .HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH)
                .HasColumnName("description");
        });
        
        builder.ComplexProperty(p => p.PetInfo, pi =>
        {
            pi.Property(pp => pp.Color).HasColumnName("color");
            pi.Property(pp => pp.HealthInfo).HasColumnName("health_info");
            pi.Property(pp => pp.Weight).HasColumnName("weight");
            pi.Property(pp => pp.Height).HasColumnName("height");
            pi.Property(pp => pp.BirthdayDate).HasColumnName("birthday_date");
            pi.Property(pp => pp.IsCastrated).HasColumnName("is_castrated");
            pi.Property(pp => pp.IsVaccinated).HasColumnName("is_vaccinated");
        });
        
        builder.ComplexProperty(p => p.Address, pi =>
        {
            pi.Property(pp => pp.City).HasColumnName("city");
            pi.Property(pp => pp.Street).HasColumnName("street");
            pi.Property(pp => pp.Country).HasColumnName("country");
        });
        
        builder.ComplexProperty(p => p.PhoneNumber, pi =>
        {
            pi.Property(pp => pp.Value).HasColumnName("phone_number");
        });

        /*builder.OwnsOne(v => v.Requisites, r =>
        {
            r.ToJson("requisites");
            r.OwnsMany(ri => ri.Requisites, rt =>
            {
                rt.Property(t => t.Description).HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH).HasJsonPropertyName("description");
                rt.Property(t => t.Name).HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH).HasJsonPropertyName("name");
            });
        });
        
        builder.OwnsOne(p => p.PetPhotosInfo, pp =>
        {
            pp.ToJson("pet_photos");
            pp.OwnsMany(ppi => ppi.PetPhotos, pt =>
            {
                
                pt.Property(petp => petp.Path)
                    .HasConversion(
                        p => p.Value,
                        value => FilePath.Create(value).Value);
                
                pt.Property(petp => petp.IsMain);
            });
        });*/
        
        builder.Property(v => v.PetPhotos)
            .HasValueObjectsJsonConversion(
                input => new PetPhotoDto() { Path = input.Path.Value },
                output => PetPhoto.Create(FilePath.Create(output.Path).Value, output.IsPhotoMain).Value)
            .HasColumnName("pet_photos");
        
        builder.Property(v => v.Requisites)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto(input.Name, input.Description),
                output => Requisite.Create(output.Name, output.Description).Value)
            .HasColumnName("requisites");

        builder.Property(p => p.PetStatus).HasColumnName("pet_status");
        builder.Property(p => p.CreationDate).HasColumnName("creation_date");
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}