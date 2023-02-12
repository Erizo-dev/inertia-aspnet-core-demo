
using System.ComponentModel.DataAnnotations;

namespace PingCrm.Host.Requests
{
    public class OrganizationRequest
    {
        [MinLength(3, ErrorMessage="at least 3")]
        public string? Name { get; set; }

        [MinLength(3, ErrorMessage ="at least 3")]
        public string? Email { get; set; } 
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }

    }
}