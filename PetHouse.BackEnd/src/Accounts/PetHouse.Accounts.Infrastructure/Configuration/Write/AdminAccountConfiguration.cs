using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Accounts.Domain.Models;

namespace PetHouse.Accounts.Infrastructure.Configuration.Write;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");

        builder.ComplexProperty(u => u.FullName, bi =>
        {
            bi.Property(b => b.Name).HasColumnName("name");
            bi.Property(b => b.Surname).HasColumnName("surname");
        });
    }
}