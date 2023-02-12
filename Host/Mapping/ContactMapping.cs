using Mapster;
using PingCrm.Host.Dto;
using PingCrm.Host.Entities;

namespace PingCrm.Host.Mapping;


public class ContactMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Contact, ContactDto>()
        .Map(dest => dest.AccountName, src => src.Account.Name)
        .Map(dest => dest.OrganizationName, src =>src.Account.Name);
    }
}
