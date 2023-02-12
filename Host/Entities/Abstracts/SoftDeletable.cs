namespace PingCrm.Host.Entities.Abstracts;

public abstract class SoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedAt { get; set; }
}