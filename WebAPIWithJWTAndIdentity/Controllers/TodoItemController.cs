using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIWithJWTAndIdentity.Models.Dtos;
using WebAPIWithJWTAndIdentity.Models.Filters;
using WebAPIWithJWTAndIdentity.Response;
using WebAPIWithJWTAndIdentity.Services.Interfaces;

namespace WebApi.Controllers;
[Route("[controller]")]
public class TodoItemController(ITodoItemService _service) : Controller
{
    [HttpGet("get-todoItem")]
    public async Task<Response<List<TodoItemDto>>> GetTodoItemAsync(TodoItemFilter filter)
    {
        return await _service.GetTodoItemAsync(filter);
    }
    
    
    [HttpPost("add-todoItem")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTodoItemAsync([FromBody]TodoItemAddDto todoItemAddDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _service.AddTodoItemAsync(todoItemAddDto);
            return StatusCode(response.StatusCode, response);
        }
        else
        {
            var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er=>er.ErrorMessage)).ToList();
            var response  =  new Response<TodoItemDto>(HttpStatusCode.BadRequest, errors);
            return StatusCode(response.StatusCode, response);
        }
        
    }
    
    
    [HttpPut("update-quote")]
    [Authorize(Roles = "Admin,Manager,Mentor")]
    public async Task<Response<TodoItemDto>> UpdateTodoItemAsync(TodoItemDto todoItemDto)
    {
        return await _service.UpdateTodoItemAsync(todoItemDto);
    }
    
    [HttpDelete("delete-quote")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTodoItemAsync(int  todoItemId)
    {
        var response = await _service.DeleteTodoItemAsync(todoItemId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-by-id-todoItem")]
    public async Task<IActionResult> GetTodoItemById(int todoItemId)
    {
        var response = await _service.GetTodoItemByIdAsync(todoItemId);
        return StatusCode(response.StatusCode, response);
    }
}