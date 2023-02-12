using Microsoft.AspNetCore.Identity;

namespace PingCrm.Host.Entities.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    // Required from the time you change IdentityUser Key
}