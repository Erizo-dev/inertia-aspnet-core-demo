using Bogus;
using Microsoft.AspNetCore.Identity;
using PingCrm.Host.Entities;
using PingCrm.Host.Entities.Identity;

namespace PingCrm.Host.Data;

public static class Seeder
{
    public static async Task SeedEntities(IServiceProvider sp)
    {
        // get db Context 
        using var scope = sp.CreateScope();
        var services = scope.ServiceProvider;
        // get the service provider and db context.
        var context = services.GetService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        if (context?.Accounts.Any() != false)
        {
            return;
        }
        var accounts = getAccountFaker().Generate(1);
        context.Accounts.AddRange(accounts);
        await context.SaveChangesAsync();
        var organizations = getOrganizationFaker(accounts[0].Id).Generate(12);
        context.Organizations.AddRange(organizations);
        await context.SaveChangesAsync();

        var contacts = new List<Contact>();
        foreach (var org in organizations)
        {
            var orgContacts = getContactFaker(accounts[0].Id, org.Id)
            .GenerateBetween(5, 6);
            contacts.AddRange(orgContacts);
        }

        context.Contacts.AddRange(contacts);
        await context.SaveChangesAsync();

        await userManager.CreateAsync(new ApplicationUser {
            AccountId = accounts[0].Id,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@doe.com",
            UserName = "JohnDoe"
        }, "Password01!");
    }
    public static Faker<ApplicationUser> getAppUserFaker(Guid accountId)
    {
        return new Faker<ApplicationUser>()
            .RuleFor(u => u.AccountId, _ => accountId)
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.PhotoPath, f => f.Image.LoremPixelUrl())
            .RuleFor(u => u.PhoneNumber, f => f.Person.Phone);
    }

    public static Faker<Account> getAccountFaker()
    {
        return new Faker<Account>()
          .RuleFor(a => a.Id, _ => Guid.NewGuid())
          .RuleFor(a => a.Name, f => f.Company.CompanyName());
    }
    public static Faker<Organization> getOrganizationFaker(Guid accountId)
    {
        return new Faker<Organization>()
       .RuleFor(o => o.Id, _ => Guid.NewGuid())
       .RuleFor(o => o.Address, f => f.Address.FullAddress())
       .RuleFor(o => o.City, f => f.Address.City())
       .RuleFor(o => o.Country, f => f.Address.Country())
       .RuleFor(o => o.Email, f => f.Person.Email)
       .RuleFor(o => o.Name, f => f.Commerce.Department())
       .RuleFor(o => o.Phone, f => f.Person.Phone)
       .RuleFor(o => o.PostalCode, f => f.Address.ZipCode())
       .RuleFor(o => o.Region, f => f.Address.State())
       .RuleFor(o => o.AccountId, _ => accountId)
       .RuleFor(o => o.IsDeleted, _ => false);
    }
    public static Faker<Contact> getContactFaker(Guid accountId, Guid OrganizationId)
    {
        return new Faker<Contact>()
                .RuleFor(c => c.Id, _ => Guid.NewGuid())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.Country, f => f.Address.County())
                .RuleFor(c => c.Email, f => f.Person.Email)
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.Phone, f => f.Person.Phone)
                .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
                .RuleFor(c => c.Region, f => f.Address.State())
                .RuleFor(c => c.AccountId, _ => accountId)
                .RuleFor(c => c.OrganizationId, _ => OrganizationId)
                .RuleFor(c => c.IsDeleted, _ => false);
    }
}