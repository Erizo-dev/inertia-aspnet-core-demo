namespace PingCrm.Host.Dto
{
    public class OrganizationDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        public string AccountName { get; set; } = null!;
        public List<ContactDto> Contacts { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}