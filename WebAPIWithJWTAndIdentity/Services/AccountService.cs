using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebAPIWithJWTAndIdentity.Data;
using WebAPIWithJWTAndIdentity.Response;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountService(
        UserManager<IdentityUser> userManager, 
        IConfiguration configuration,
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
        _roleManager = roleManager;
    }
    
    public async Task<Response<RegisterDto>> Register(RegisterDto model)
    {
        var mapped = new IdentityUser()
        {
            UserName = model.Username,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };

       
        var response = await _userManager.CreateAsync(mapped,model.Password);
        if (response.Succeeded == true)
            return new Response<RegisterDto>(model);
        else return new Response<RegisterDto>(HttpStatusCode.BadRequest, "something is wrong");

    }

    public async Task<Response<string>> AddOrRemoveUserFromRole(UserRoleDto userRole, bool delete = false)
    {
        var role = await _roleManager.FindByIdAsync(userRole.RoleId);
        var user = await _userManager.FindByIdAsync(userRole.UserId);
        if (delete == true)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            return new Response<string>(HttpStatusCode.OK, "removed");
        }
        var userInRole = await _userManager.IsInRoleAsync(user, role.Name);
        if (userInRole == true) return new Response<string>(HttpStatusCode.BadRequest, "Role exists");
        await _userManager.AddToRoleAsync(user, role.Name);
        return new Response<string>(HttpStatusCode.OK, "done");
    }
    

    
    public async Task<Response<string>> Login(LoginDto login)
    {
        var user = await _userManager.FindByNameAsync(login.Username);
        if (user != null)
        {
            var checkPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if (checkPassword)
            {
                var token  =  await GenerateJwtToken(user);
                return new Response<string>(token);
            }
            else
            {
                return new Response<string>(HttpStatusCode.BadRequest, "login or password is incorrect");
            }
            
        }

        return new Response<string>(HttpStatusCode.BadRequest, "login or password is incorrect");
    }
    
    //Method to generate The Token
    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id),
        };
        
        //add roles
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

       //Making true structure by Abubakr
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
    
    
}