using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Accounts.Domain.Models;

namespace PetHouse.Accounts.Infrastructure.Configuration.Write;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");
        
        builder.ComplexProperty(u => u.FullName, bi =>
        {
            bi.Property(b => b.Name).HasColumnName("name");
            bi.Property(b => b.Surname).HasColumnName("surname");
        });
    }
}