﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.SharedKernel.Id;
using PetHouse.SpecieManagement.Domain.Entities;

namespace PetHouse.SpecieManagement.Infrastructure.Configuration.Write;

public partial class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(s => s.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value)
            );
        
        builder.ComplexProperty(s => s.Name, sn =>
        {
            sn.Property(n => n.Value).HasColumnName("name");
        });
    }
}