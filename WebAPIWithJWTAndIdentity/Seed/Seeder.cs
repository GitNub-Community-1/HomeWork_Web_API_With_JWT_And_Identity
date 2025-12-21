using Microsoft.AspNetCore.Identity;
using WebAPIWithJWTAndIdentity.Data;

namespace Infrastructure.Seed;

public class Seeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public Seeder(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> SeedRole()
    {
        var newroles = new List<IdentityRole>()
        {
            new IdentityRole(Roles.Admin),
            new IdentityRole(Roles.Marketing),
            new IdentityRole(Roles.Finance),
            new IdentityRole(Roles.Mentor),
            new IdentityRole(Roles.Student),

        };

        var existing = _roleManager.Roles.ToList();
        foreach (var role in newroles)
        {
            if (existing.Exists(e => e.Name == role.Name) == false)
            {
                await _roleManager.CreateAsync(role);
            }
        }

        return true;

    }

    public async Task<bool> SeedUser()
    {
        var existing = await _userManager.FindByNameAsync("admin");
        if (existing != null) return false;
        
        var identity = new IdentityUser()
        {
            UserName = "admin",
            PhoneNumber = "13456777",
            Email = "admin@gmail.com"
        };

        var result = await _userManager.CreateAsync(identity, "hello123");
        await _userManager.AddToRoleAsync(identity, Roles.Admin);
        return result.Succeeded;
    }
    
   

}

public class Roles
{
    public const string Admin = "Admin";
    public const string Marketing = "Marketing";
    public const string Finance = "Finance";
    public const string Mentor = "Mentor";
    public const string Student = "Student";
}