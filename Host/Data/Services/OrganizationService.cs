using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using PingCrm.Host.Dto;
using PingCrm.Host.Entities;
using PingCrm.Host.Requests;

namespace PingCrm.Host.Data.Services;


public class OrganizationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrganizationService> _logger;
    private readonly IMapper _mapper;

    public OrganizationService(ILogger<OrganizationService> logger,
    ApplicationDbContext context,
    IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public Organization GetOrganization(Guid orgId)
    {
        var organization = _context.Organizations
        .Include(o => o.Contacts)
        .Include(o => o.Account)
        .First(o => o.Id == orgId);
        if (organization is null)
        {
            throw new Exception("Organization not found");
        }
        return organization;
    }
    public List<Organization> GetAllOrganizations()
    {
        return _context.Organizations.Include(o => o.Account).ToList();
    }
    public IQueryable ListOrGanizations(SearchFilterDto filter)
    {
        var pagedQuery = _context.Organizations.AsQueryable();

        if (!string.IsNullOrEmpty(filter?.Search))
        {
            // ef does not seem to support case insensitive Contains
            pagedQuery = pagedQuery.Where(o => o.Name.ToLower().Contains(filter.Search.ToLower()));
        }
        if (filter is not null)
        {
            pagedQuery = filter.Trashed switch
            {
                "with" => pagedQuery,
                "only" => pagedQuery.Where(o => o.IsDeleted),
                _ => pagedQuery.Where(o => !o.IsDeleted)
            };
        }

        return pagedQuery;
    }

    public void UpdateOrganization(OrganizationRequest request, Guid orgId)
    {
        var org = _context.Organizations.Find(orgId);
        if (org is null)
        {
            throw new Exception("Organization not found");
        }
        try
        {
            // mapping all fields from the request
            request.Adapt(org);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update org {}", ex.Message);
            throw;
        }
    }

    public void DeleteOrganization(Guid orgId)
    {
        var org = _context.Organizations.Find(orgId);
        if (org is null)
        {
            throw new Exception("Organization not found");
        }
        try
        {
            // org.IsDeleted = true;
            _context.Organizations.Remove(org);
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public void RestoreOrganization(Guid orgId)
    {

        var org = _context.Organizations.Find(orgId);
        if (org is null)
        {
            throw new Exception("Organization not found");
        }
        try
        {
            org.IsDeleted = true;
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            throw;
        }
    }


    public Organization CreateOrganization(string username, OrganizationRequest request)
    {
        try
        {
            var user = _context.Users.First(u => u.UserName == username);
            var org = request.Adapt<Entities.Organization>();
            org.AccountId = user.AccountId;
            _context.Organizations.Add(org);
            _context.SaveChanges();
            return org;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add org {}", ex.Message);
            throw;
        }
    }
}