using Mapster;
using PingCrm.Host.Dto;
using PingCrm.Host.Entities;
using PingCrm.Host.Entities.Identity;

namespace PingCrm.Host.Mapping;

public class UserMApping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, UserDto>()
        .Map( dest => dest.Owner, src => src.IsOwner)
        .Map(dest => dest.DeletedAt, src => DateTimeOffset.UtcNow);
        // TODO add deleted at field
    }
}

