using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PingCrm.Host.Entities.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.HasMany(a => a.Contacts).WithOne(c => c.Account).HasForeignKey(c => c.AccountId);
        builder.HasMany(a => a.Organizations).WithOne(o => o.Account).HasForeignKey(o => o.AccountId);

        builder.Property(a => a.Name).HasMaxLength(255);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.Property(a => a.CreatedAt).HasDefaultValue(DateTimeOffset.UtcNow);
        builder.Property(a => a.UpdatedAt).HasDefaultValue(DateTimeOffset.UtcNow);
    }
}