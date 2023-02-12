using Mapster;
using PingCrm.Host.Dto;
using PingCrm.Host.Entities;

namespace PingCrm.Host.Mapping;

public class OraganizationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Organization, OrganizationDto>()
        .Map( dest => dest.AccountName, src => src.Account.Name);
    }
}
