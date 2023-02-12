using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PingCrm.Host.Entities.Configuration;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();
        builder.HasMany(o => o.Contacts ).WithOne(c => c.Organization).HasForeignKey(c => c.OrganizationId).OnDelete(DeleteBehavior.Cascade);


        builder.Property( o => o.Address).HasMaxLength(255);
        builder.Property(o => o.City).HasMaxLength(255);
        builder.Property(o => o.Country).HasMaxLength(100);
        builder.Property(o => o.Email).HasMaxLength(255);
        builder.Property(o => o.Name).HasMaxLength(255);
        builder.Property(o => o.Phone).HasMaxLength(255);
        builder.Property(o => o.PostalCode).HasMaxLength(255);
        builder.Property(o => o.Region).HasMaxLength(255);
        builder.Property(o => o.IsDeleted).HasDefaultValue(false);

        builder.Property(o => o.CreatedAt).HasDefaultValue(DateTimeOffset.UtcNow);
        builder.Property(o => o.UpdatedAt).HasDefaultValue(DateTimeOffset.UtcNow);
    }
}