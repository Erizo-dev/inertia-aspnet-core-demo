using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using PingCrm.Host.Dto;
using PingCrm.Host.Entities;
using PingCrm.Host.Requests;

namespace PingCrm.Host.Data.Services;

public class ContactService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContactService> _logger;

    public ContactService(ILogger<ContactService> logger, ApplicationDbContext context)
    {
        _context = context;
        _logger = logger;
    }
  public Contact GetContact(Guid contactId)
    {
        var contact = _context.Contacts
        .Include(c => c.Organization)
        .Include(c => c.Account)
        .First(c => c.Id == contactId);
        if (contact is null)
        {
            throw new Exception("Contact not found");
        }
        return contact;
    }

    public IQueryable ListContacts(SearchFilterDto filter)
    {
        var pagedQuery = _context.Contacts.Include(c => c.Organization).AsQueryable();

        if (!string.IsNullOrEmpty(filter?.Search))
        {
            // ef does not seem to support case insensitive Contains
            // to be edited 
            // search by something relevant 
            // TODO
            pagedQuery = pagedQuery.Where(c => c.FirstName.ToLower().Contains(filter.Search.ToLower()));
        }
        if (filter is not null)
        {
            pagedQuery = filter.Trashed switch
            {
                "with" => pagedQuery,
                "only" => pagedQuery.Where(c => c.IsDeleted),
                _ => pagedQuery.Where(c => !c.IsDeleted)
            };
        }

        return pagedQuery;
    }

    public void UpdateContact(ContactRequest request, Guid contactId)
    {
        var contact = _context.Contacts.Find(contactId);
        if (contact is null)
        {
            throw new Exception("Contact not found");
        }
        try
        {
            // mapping all fields from the request
            request.Adapt(contact);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update contact {}", ex.Message);
            throw;
        }
    }

    public void DeleteContact(Guid contactId)
    {
        var contact = _context.Contacts.Find(contactId);
        if (contact is null)
        {
            throw new Exception("Contact not found");
        }
        try
        {
            // soft deletions
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public void RestoreContact(Guid contactId)
    {

        var contact = _context.Contacts.Find(contactId);
        if (contact is null)
        {
            throw new Exception("Contact not found");
        }
        try
        {
            contact.IsDeleted = false;
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            throw;
        }
    }


    public Contact CreateContact( ContactRequest request)
    {
        try
        {
            var orgId = request.OrganizationId;
            var org = _context.Organizations.Find(orgId);
            var contact = request.Adapt<Contact>();
            contact.AccountId = org!.AccountId;
            contact.OrganizationId = orgId;
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return contact;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add contact {}", ex.Message);
            throw;
        }
    }
}