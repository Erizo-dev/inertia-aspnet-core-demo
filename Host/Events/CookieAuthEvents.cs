using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Host.Events
{
    public class CookieAuthEvents : CookieAuthenticationEvents
    {
        private readonly ILogger<CookieAuthEvents> _logger;

        public CookieAuthEvents(ILogger<CookieAuthEvents> logger)
        {
            _logger = logger;
        }
        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.LogInformation("Access denied redirection Event");
            context.Response.Headers.Remove("Location");
            context.Response.Headers.Location = "/login";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return base.RedirectToAccessDenied(context);
        }
    }
}