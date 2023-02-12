using System.Security.Claims;
using System.Text.Json;
using Host.Events;
using InertiaCore;
using InertiaCore.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PingCrm.Host.Data;
using PingCrm.Host.Entities.Identity;
using PingCrm.Host.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

var connectionString = builder.Configuration.GetConnectionString("local");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddDataServices();
builder.Services.AddMapster();
builder.Services.AddTransient<CookieAuthEvents>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
// .AddCookie(options =>
// {
//     options.EventsType = typeof(CookieAuthEvents);
//     options.LoginPath = new PathString("/auth/login");
//     options.LogoutPath = new PathString("/auth/logout");
//     options.AccessDeniedPath = new PathString("/auth/login");
//     options.Cookie.Name = "auth_cookie";
//     options.SlidingExpiration = true;
// });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/login";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    // options.EventsType = typeof(CookieAuthEvents);
});

builder.Services.AddControllersWithViews();
builder.Services.AddInertia(options =>
{
    options.SsrEnabled = true;
    // You can optionally set a different URL than the default.
    options.SsrUrl = "http://127.0.0.1:13714/render"; // default
    options.RootView = "~/Views/App.cshtml";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.Use((context, next) =>
{
    // https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1.getuserid?view=aspnetcore-7.0
    string? name = context.User!.Identity!.Name;
    var id = context.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    Inertia.Share("auth", new
    {
        UserName = name,
        UserId = id,

    });
    return next(context);
});

app.UseInertia();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// seed before running the app
await Seeder.SeedEntities(app.Services);
Console.WriteLine("database seeded, application will run ");
app.Run();
