using Microsoft.AspNetCore.Mvc;
using InertiaCore;
using PingCrm.Host.Dto;
using X.PagedList;
using PingCrm.Host.Requests;
using System.Text.Json;
using Mapster;
using MapsterMapper;
using PingCrm.Host.Data.Services;

namespace PingCrm.Host.Controllers;

[Route("[controller]")]
public class OrganizationsController : Controller
{
    private readonly ILogger<OrganizationsController> _logger;
    private readonly IMapper _mapper;
    private readonly OrganizationService _organizationService;

    public OrganizationsController(ILogger<OrganizationsController> logger,
    IMapper mapper,
    OrganizationService organizationService)
    {
        _logger = logger;
        _mapper = mapper;
        _organizationService = organizationService;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] SearchFilterDto filter = default)
    {
        _logger.LogInformation("Organizations index called ");

        var pagedQuery = _organizationService.ListOrGanizations(filter);
        var paged = pagedQuery.ProjectToType<OrganizationDto>()
            .ToPagedList(page, perPage);

        // Component path seems to be case sensitive 
        return Inertia.Render("organizations/Search", new
        {
            Organizations = new
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
        _logger.LogInformation("Organizations create page");

        return Inertia.Render("organizations/Create");
    }

    [HttpPost("create")]
    public IActionResult Store([FromBody] OrganizationRequest request)
    {
        _logger.LogInformation("Org request {}", JsonSerializer.Serialize(request));
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Org validation error");
            // rtfm call the method directly
            Inertia.Share("flash", new { Error = "Org could not be created" });
            return Create();
        }
        var username = User!.Identity!.Name;
        _= _organizationService.CreateOrganization(username!, request);

        // Flashes the message by adding to the shared data ? 
        // Nb Shared keys are kept with this capitalization in propes
        Inertia.Share("flash", new { Success = "Org Created" });
        return RedirectToAction("Index");
    }

    [HttpGet("{orgId:guid}/edit")]
    public IActionResult Edit(Guid orgId)
    {
        var organization = _organizationService.GetOrganization(orgId);
        return Inertia.Render("organizations/Edit", new { Organization = _mapper.Map<OrganizationDto>(organization) });
    }
    [HttpPut("{orgId:guid}/edit")]
    public IActionResult Update(Guid orgId, [FromBody] OrganizationRequest request)
    {
        _logger.LogInformation("Org edit request {}", JsonSerializer.Serialize(request));
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Org validation error");

            Inertia.Share("flash", new { Error = "Org could not be created" });
            return Edit(orgId);
        }

        _organizationService.UpdateOrganization(request, orgId);
        Inertia.Share("flash", new { Success = "Org Updated" });
        return RedirectToAction("Index");
    }


    [HttpDelete("{orgId:guid}")]
    public IActionResult Destroy(Guid orgId)
    {
        _organizationService.DeleteOrganization(orgId);
        Inertia.Share("flash", new { Success = "Organization deleted" });
        return RedirectToAction("Index");
    }

    [HttpPost("{orgId:guid}/restore")]
    public IActionResult Restore(Guid orgId)
    {
        _organizationService.RestoreOrganization(orgId);
        Inertia.Share("flash", new { Success = "Org Restored" });
        return RedirectToAction("Index");
    }
}
