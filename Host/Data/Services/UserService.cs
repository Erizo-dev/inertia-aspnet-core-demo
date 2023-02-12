using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PingCrm.Host.Dto;
using PingCrm.Host.Entities.Identity;
using PingCrm.Host.Requests;

namespace PingCrm.Host.Data.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(
        ILogger<UserService> logger,
         IMapper mapper,
          ApplicationDbContext context,
          UserManager<ApplicationUser> userManager
          )
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public ApplicationUser GetUser(Guid userId)
    {
        var user = _context.Users
        .Include(u => u.Account)
        .First(o => o.Id == userId);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        return user;
    }
    public List<ApplicationUser> GetAllUsers()
    {
        return _context.Users.Include(u => u.Account).ToList();
    }
    public IQueryable ListUsers(SearchFilterDto filter)
    {
        var pagedQuery = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(filter?.Search))
        {
            // ef does not seem to support case insensitive Contains
            // TODO update search to something more meaningful
            pagedQuery = pagedQuery.Where(o => o.FirstName.ToLower().Contains(filter.Search.ToLower()));
        }
 
        if (filter is not null)
        {
            pagedQuery = filter.Trashed switch
            {
                "with" => pagedQuery,
                "only" => pagedQuery.Where(u => u.IsDeleted),
                _ => pagedQuery.Where(u => !u.IsDeleted)
            };
            pagedQuery = filter.Role switch
            {
                "user" => pagedQuery.Where(u => !u.IsOwner),
                "owner" => pagedQuery.Where(u => u.IsOwner),
                _ => pagedQuery,
            };
        }
        return pagedQuery;
    }



    public async Task<ApplicationUser> CreateUser(string creatorName, UserRequest request)
    {

        var user = _context.Users.First(u => u.UserName == creatorName);
        var result = await _userManager.CreateAsync(new ApplicationUser
        {
            AccountId = user.AccountId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.FirstName,
        }, request!.Password);

        if (result.Errors.Any())
        {
            return null;
        }

        ApplicationUser? applicationUser = await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Email == request.Email);
        return applicationUser!;
    }


    public void UpdateUser(UserRequest request, Guid userId)
    {
        var user = _context.Users.Find(userId);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        try
        {
            // cannot map all fields 
            // business logic
            // won't allow for password change
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsOwner = request.IsOwner;

            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user {}", ex.Message);
            throw;
        }
    }
    public void DeleteUser(Guid userId)
    {
        var user = _context.Users.Find(userId);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        try
        {
            // does not actually extends SoftDeletable
            user.IsDeleted = true;
            user.DeletedAt = DateTimeOffset.UtcNow;
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public void RestoreUser(Guid userId)
    {
        var user = _context.Users.Find(userId);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        try
        {
            user.IsDeleted = false;
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

}