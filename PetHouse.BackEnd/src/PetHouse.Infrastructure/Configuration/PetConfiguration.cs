using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Domain;
using PetHouse.Domain.Constraints;

namespace PetHouse.Infrastructure.Configuration;

public partial class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets");
        
        builder.HasKey(p => p.Id);

        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(DefaultConstraints.MAX_NAME_LENGTH);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(DefaultConstraints.MAX_DESCRIPTION_LENGTH);
        builder.Property(p => p.Address).IsRequired();
        builder.Property(p => p.PhoneNumber).IsRequired();
        builder.Property(p => p.PetStatus).IsRequired();
        builder.Property(p => p.CreationDate).IsRequired();

        builder.OwnsMany(p => p.Requisites, r =>
        {
            r.Property(rt => rt.Description).IsRequired();
            r.Property(rt => rt.Name).IsRequired();
        });
        
        builder.OwnsMany(p => p.PetPhotos, pp =>
        {
            pp.HasKey(ppt => ppt.Id);
            pp.Property(ppt => ppt.Path).IsRequired();
            pp.Property(ppt => ppt.IsMain).IsRequired();
        });
    }
}