
using System.ComponentModel.DataAnnotations;

namespace PingCrm.Host.Requests;
public class UserRequest
{
    [MaxLength(255)]
    public string FirstName { get; set; } = null!;
    [MaxLength(255)]
    public string LastName { get; set; } = null!;
    [MaxLength(255)]
    public string? PhotoPath { get; set; }

    [MaxLength(255)]
    public string Email { get; set; } = null!;

    public bool IsOwner { get; set; } = false;

    public Guid AccountId { get; set; }

    public string? Password { get; set; }

}