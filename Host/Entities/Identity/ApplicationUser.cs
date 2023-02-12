using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using PingCrm.Host.Entities.Abstracts;

namespace PingCrm.Host.Entities.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    // Prefering guid as keys out of habbit
    [MaxLength(255)]
    public string FirstName { get; set; } = null!;
    [MaxLength(255)]
    public string LastName { get; set; } = null!;
    [MaxLength(255)]
    public string? PhotoPath { get; set; }

    public bool IsOwner { get; set; } = false;

    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }

    // Navigations
    public Account Account { get; set; } = null!;

    // Also soft deletable
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset DeletedAt { get; set; }
}