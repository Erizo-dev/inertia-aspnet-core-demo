using System.Text.Json;
using InertiaCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PingCrm.Host.Data.Services;
using PingCrm.Host.Dto;
using PingCrm.Host.Requests;
using X.PagedList;

namespace PingCrm.Host.Controllers;

[Route("[controller]")]
public class ContactsController : Controller
{
    private readonly ILogger<ContactsController> _logger;
    private readonly IMapper _mapper;
    private readonly ContactService _contactService;
    private readonly OrganizationService _organizationService;

    public ContactsController(ILogger<ContactsController> logger,
    IMapper mapper,
    ContactService contactService, OrganizationService organizationService)
    {
        _organizationService = organizationService;
        _logger = logger;
        _mapper = mapper;
        _contactService = contactService;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] SearchFilterDto filter = default)
    {
        _logger.LogInformation("Contacts index called ");

        var pagedQuery = _contactService.ListContacts(filter);
        var paged = pagedQuery.ProjectToType<ContactDto>()
            .ToPagedList(page, perPage);

        // Component path seems to be case sensitive 
        return Inertia.Render("contacts/Search", new
        {
            Contacts = new
            {
                Data = paged,
                Links = new
                {
                    paged.IsFirstPage,
                    paged.IsLastPage,
                    paged.PageCount,
                    paged.PageNumber,
                    paged.PageSize,
                    paged.HasNextPage,
                    paged.HasPreviousPage
                }

            },
            Filters = filter
        });
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        _logger.LogInformation("Contacts create page");
        var organizations = _organizationService.GetAllOrganizations();

        return Inertia.Render("contacts/Create", new { Organizations = _mapper.Map<List<OrganizationDto>>(organizations)} );
    }

    [HttpPost("create")]
    public IActionResult Store([FromBody] ContactRequest request)
    {
        _logger.LogInformation("Contact request {}", JsonSerializer.Serialize(request));
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Org validation error");
            // rtfm call the method directly
            Inertia.Share("flash", new { Error = "Contact could not be created" });
            return Create();
        }
        _ = _contactService.CreateContact(request);

        // Flashes the message by adding to the shared data ? 
        // Nb Shared keys are kept with this capitalization in propes
        Inertia.Share("flash", new { Success = "Contact Created" });
        return RedirectToAction("Index");
    }

    [HttpGet("{contactId:guid}/edit")]
    public IActionResult Edit(Guid contactId)
    {
        var contact = _contactService.GetContact(contactId);
        var organizations = _organizationService.GetAllOrganizations();
        return Inertia.Render("contacts/Edit", new { Contact = _mapper.Map<ContactDto>(contact), Organizations = _mapper.Map<List<OrganizationDto>>(organizations) });
    }
    [HttpPut("{contactId:guid}/edit")]
    public IActionResult Update(Guid contactId, [FromBody] ContactRequest request)
    {
        _logger.LogInformation("Contact edit request {}", JsonSerializer.Serialize(request));
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Contact validation error");

            Inertia.Share("flash", new { Error = "Contact could not be created" });
            return Edit(contactId);
        }

        _contactService.UpdateContact(request, contactId);
        Inertia.Share("flash", new { Success = "Contact Updated" });
        return RedirectToAction("Index");
    }

    [HttpDelete("{contactId:guid}")]
    public IActionResult Destroy(Guid contactId)
    {
        _contactService.DeleteContact(contactId);
        Inertia.Share("flash", new { Success = "Contact deleted" });
        return RedirectToAction("Index");
    }

    [HttpPost("{contactId:guid}/restore")]
    public IActionResult Restore(Guid contactId)
    {
        _contactService.RestoreContact(contactId);
        Inertia.Share("flash", new { Success = "Contact Restored" });
        return RedirectToAction("Index");
    }
}