using PingCrm.Host.Entities.Abstracts;

namespace PingCrm.Host.Entities;

public class Account : SoftDeletable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // Navigation
    public ICollection<Contact>? Contacts { get; set; } 
    public ICollection<Organization>? Organizations { get; set; }
}