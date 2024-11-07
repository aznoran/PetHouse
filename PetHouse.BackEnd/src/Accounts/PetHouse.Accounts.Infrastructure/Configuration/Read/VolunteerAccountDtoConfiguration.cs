using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Accounts.Contracts.Dtos;
using PetHouse.Core.Dtos.PetManagment;

namespace PetHouse.Accounts.Infrastructure.Configuration.Read;

public class VolunteerAccountDtoConfiguration : IEntityTypeConfiguration<VolunteerAccountDto>
{
    public void Configure(EntityTypeBuilder<VolunteerAccountDto> builder)
    {
        builder.ToTable("volunteer_accounts");

        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Requisites)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IReadOnlyList<RequisiteDto>>
                    (json, JsonSerializerOptions.Default)!);
        
        builder.Property(v => v.Certificates)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IReadOnlyList<CertificateDto>>
                    (json, JsonSerializerOptions.Default)!);
    }
}