using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PingCrm.Host.Entities;
using PingCrm.Host.Entities.Abstracts;
using PingCrm.Host.Entities.Identity;

namespace PingCrm.Host.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public override int SaveChanges()
    {
        HandleSoftDelete();
        return base.SaveChanges();
    }
    private void HandleSoftDelete()
    {
        var entities = ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Deleted);
        foreach (var entity in entities)
        {
            if (entity.Entity is SoftDeletable softDeletable)
            {
                entity.State = EntityState.Modified;
                softDeletable.IsDeleted = true;
            }
        }
    }
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Organization> Organizations { get; set; } = null!;
}