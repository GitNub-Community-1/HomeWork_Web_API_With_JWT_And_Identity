using System.Net;
using Domain.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIWithJWTAndIdentity.Response;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {

        //Making true structure by  Abubakr
        _accountService = accountService;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
    {
        if (ModelState.IsValid)
        {
            var response  = await _accountService.Register(registerDto);
            return StatusCode(response.StatusCode, response);
        }
        else
        {
            var errorMessages = ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            var response = new Response<RegisterDto>(HttpStatusCode.BadRequest, errorMessages);
            return StatusCode(response.StatusCode, response);
        }
        
    }
    
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody]LoginDto registerDto)
    {
        if (ModelState.IsValid)
        {
            var response  = await _accountService.Login(registerDto);
            return StatusCode(response.StatusCode, response);
        }
        else
        {
            var errorMessages = ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            var response = new Response<RegisterDto>(HttpStatusCode.BadRequest, errorMessages);
            return StatusCode(response.StatusCode, response);
        }
        
    }

    [HttpPost("AddUserToRole")]
    public async Task<Response<string>> AddUserToRole(UserRoleDto userRoleDto)
    {
        return await _accountService.AddOrRemoveUserFromRole(userRoleDto,false);
    }
    
    
    [HttpDelete("DeleteRoleFromUser")]
    public async Task<Response<string>> DeleteRoleFromUser(UserRoleDto userRoleDto)
    {
        return await _accountService.AddOrRemoveUserFromRole(userRoleDto,true);
    }





}
