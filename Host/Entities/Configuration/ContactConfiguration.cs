using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PingCrm.Host.Entities.Configuration;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.HasOne(c => c.Organization).WithMany( o => o.Contacts).HasForeignKey(c => c.OrganizationId);

        builder.Property(c =>c.Address).HasMaxLength(255);
        builder.Property(c => c.City).HasMaxLength(255);
        builder.Property(c => c.Country).HasMaxLength(255);
        builder.Property(c => c.Email).HasMaxLength(255);
        builder.Property(c => c.FirstName).HasMaxLength(255);
        builder.Property(c => c.LastName).HasMaxLength(255);
        builder.Property(c => c.Phone).HasMaxLength(255);
        builder.Property(c => c.PostalCode).HasMaxLength(255);
        builder.Property(c => c.Region).HasMaxLength(255);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.Property(c => c.CreatedAt).HasDefaultValue(DateTimeOffset.UtcNow);
        builder.Property(c => c.UpdatedAt).HasDefaultValue(DateTimeOffset.UtcNow);
    }
}