namespace PingCrm.Host.Requests;
using System.ComponentModel.DataAnnotations;

public class ContactRequest
{
    [MinLength(3, ErrorMessage="at least 3")]
    public string FirstName { get; set; } = null!;
    [MinLength(3, ErrorMessage="at least 3")]
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }

    public Guid OrganizationId { get; set; }
}