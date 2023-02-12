namespace PingCrm.Host.Dto
{
    public class ContactDto
    {
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // foreign keys
    public Guid AccountId { get; set; }
    public Guid OrganizationId { get; set; }

    // navigation

    public string OrganizationName { get; set; } = null!;
    public string AccountName { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}