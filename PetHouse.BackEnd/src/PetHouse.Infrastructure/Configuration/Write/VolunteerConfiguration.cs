using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Application.Dtos.PetManagment;
using PetHouse.Domain.PetManagement.Aggregate;
using PetHouse.Domain.Shared.Constraints;
using PetHouse.Domain.Shared.Id;
using PetHouse.Domain.Shared.ValueObjects;
using PetHouse.Infrastructure.Extensions;

namespace PetHouse.Infrastructure.Configuration.Write;

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

        builder.ComplexProperty(v => v.FullName, vb =>
        {
            vb.Property(vp => vp.Name).IsRequired().HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH)
                .HasColumnName("name");
            vb.Property(vp => vp.Surname).IsRequired().HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH)
                .HasColumnName("surname");
        });
        
        builder.ComplexProperty(v => v.Email, vb =>
        {
            vb.Property(vp => vp.Value)
                .IsRequired()
                .HasColumnName("email");
        });
        
        builder.ComplexProperty(v => v.Description, vb =>
        {
            vb.Property(vp => vp.Value)
                .IsRequired()
                .HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH)
                .HasColumnName("description");
        });
        
        builder.ComplexProperty(v => v.YearsOfExperience, vb =>
        {
            vb.Property(vp => vp.Value)
                .IsRequired()
                .HasColumnName("years_of_experience");
        });
        
        builder.ComplexProperty(v => v.PhoneNumber, vb =>
        {
            vb.Property(vp => vp.Value)
                .IsRequired()
                .HasColumnName("phone_number");
        });

        builder.HasMany(v => v.Pets)
            .WithOne(p => p.Volunteer)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(v => v.SocialNetworks)
            .HasValueObjectsJsonConversion(
                input => new SocialNetworksDto(input.Link, input.Name),
                output => SocialNetwork.Create(output.Link, output.Name).Value)
            .HasColumnName("social_networks");

        builder.Property(v => v.Requisites)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto(input.Name, input.Description),
                output => Requisite.Create(output.Name, output.Description).Value)
            .HasColumnName("requisites");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}