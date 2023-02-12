using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InertiaCore;
using Host.Requests;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using PingCrm.Host.Entities.Identity;
using PingCrm.Host.Controllers;

namespace Host.Controllers.auth
{

    public class SessionsController : Controller
    {
        private readonly ILogger<SessionsController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SessionsController(ILogger<SessionsController> logger,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public IActionResult Create()
        {
            Inertia.Share("user", new { });
            return Inertia.Render("Login");
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<IActionResult> Store([FromBody] LoginRequest request)
        {
            _logger.LogInformation("POST Login Request {}", JsonSerializer.Serialize(request));

            // get user by email 
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return RedirectToAction(nameof(Create));
            }
            _logger.LogInformation("User found {}", user);
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.Remember, false);
            _logger.LogInformation("Auth result {}", result);
            Inertia.Share("auth", user);

            return Redirect("/");
        }

        [Authorize]
        [HttpDelete("/logout")]
        public async Task<IActionResult> Destroy()
        {
            _logger.LogInformation("POST Logout Request for {}", User!.Identity!.Name);

            await _signInManager.SignOutAsync();
            return Redirect("/login");
        }
    }
}