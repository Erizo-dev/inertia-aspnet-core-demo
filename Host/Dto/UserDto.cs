namespace PingCrm.Host.Dto;

public class UserDto
{
    public string   Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhotoPath { get; set; }
    public bool? Owner { get; set; }
    public DateTimeOffset  DeletedAt { get; set; }
    public bool IsDeleted { get; set; }

}