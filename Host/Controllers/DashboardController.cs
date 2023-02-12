using System.Security.Claims;
using System.Text.Json;
using InertiaCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PingCrm.Host.Data;

namespace PingCrm.Host.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly ApplicationDbContext _context;

    public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Route("/")]
    public IActionResult Index()
    {
        string? name = User!.Identity!.Name;
        var claims = JsonSerializer.Serialize(User!.Claims, new JsonSerializerOptions{
            // MaxDepth = 5,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        });
        var data = new { Id = 1, User = name, Claim = claims };
        return Inertia.Render("Dashboard", data);
    }
}