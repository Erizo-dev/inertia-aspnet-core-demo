using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using InertiaCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PingCrm.Host.Data.Services;
using PingCrm.Host.Dto;
using PingCrm.Host.Requests;
using X.PagedList;

namespace PingCrm.Host.Controllers;

[Route("[controller]")]
public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UsersController(ILogger<UsersController> logger,
     IMapper mapper,
      UserService userService)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] SearchFilterDto filter = default)
    {
        _logger.LogInformation("Users index called ");

        var pagedQuery = _userService.ListUsers(filter);
        var paged = pagedQuery.ProjectToType<UserDto>()
            .ToPagedList(page, perPage);

        _logger.LogInformation("Users {}", System.Text.Json.JsonSerializer.Serialize(paged));

        // Component path seems to be case sensitive 
        return Inertia.Render("users/Search", new
        {
            Users = new
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
        _logger.LogInformation("Users create page");

        return Inertia.Render("users/Create");
    }

    [HttpPost("create")]
    public IActionResult Store([FromBody] UserRequest request)
    {

        // password validation
        if (string.IsNullOrEmpty(request?.Password) || request.Password.Length < 8)
        {
            ModelState.AddModelError("password", "too weak");
        }
        _logger.LogInformation("User request {}", JsonSerializer.Serialize(request));
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("User validation error");
            // rtfm call the method directly
            Inertia.Share("flash", new { Error = "User could not be created" });
            return Create();
        }
        var username = User!.Identity!.Name;
        _ = _userService.CreateUser(username, request);

        // Flashes the message by adding to the shared data ? 
        // Nb Shared keys are kept with this capitalization in propes
        Inertia.Share("flash", new { Success = "User Created" });
        return RedirectToAction("Index");
    }

    [HttpGet("edit/{userId:guid}")]
    public IActionResult Edit(Guid userId)
    {
        _logger.LogWarning("Edit request for {user}", userId);
        var user = _userService.GetUser(userId);
        return Inertia.Render("users/Edit", new { User = _mapper.Map<UserDto>(user) });
    }
    [HttpPut("edit/{userId:guid}")]
    public IActionResult Update(Guid userId, [FromBody] UserRequest request)
    {
        _logger.LogInformation("User edit request {}", JsonSerializer.Serialize(request));
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("User validation error");

            Inertia.Share("flash", new { Error = "User could not be created" });
            return Edit(userId);
        }

        _userService.UpdateUser(request, userId);
        Inertia.Share("flash", new { Success = "User Updated" });
        return RedirectToAction("Index");
    }

    [HttpDelete("delete/{userId:guid}")]
    public IActionResult Destroy(Guid userId)
    {
        _logger.LogWarning("Will delete user {user}", userId);
        _userService.DeleteUser(userId);
        Inertia.Share("flash", new { Success = "User deleted" });
        return RedirectToAction("Index");
    }

    [HttpPut("restore/{userId:guid}")]
    public IActionResult Restore(Guid userId)
    {
        _userService.RestoreUser(userId);
        Inertia.Share("flash", new { Success = "User Restored" });
        return RedirectToAction("Index");
    }
}