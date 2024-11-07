using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Accounts.Contracts.Dtos;
using PetHouse.Accounts.Domain.Models;
using PetHouse.Core.Dtos.PetManagment;
using PetHouse.Core.Extensions;
using PetHouse.SharedKernel.ValueObjects;

namespace PetHouse.Accounts.Infrastructure.Configuration.Write;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");
        
        builder.Property(u => u.Requisites)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto(input.Name, input.Description),
                output => Requisite.Create(output.Name, output.Description).Value)
            .HasColumnName("requisites");
        
        builder.Property(v => v.Certificates)
            .HasValueObjectsJsonConversion(
                input => new CertificateDto(){Name = input.Name},
                dto => Certificate.Create(dto.Name).Value)
            .HasColumnName("certificates");
        
        builder.ComplexProperty(u => u.FullName, bi =>
        {
            bi.Property(b => b.Name).HasColumnName("name");
            bi.Property(b => b.Surname).HasColumnName("surname");
        });
    }
}