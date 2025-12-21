using Microsoft.AspNetCore.Mvc;
using WebAPIWithJWTAndIdentity.Models.Dtos;
using WebAPIWithJWTAndIdentity.Models.Filters;
using WebAPIWithJWTAndIdentity.Response;
namespace WebAPIWithJWTAndIdentity.Services.Interfaces;

public interface ITodoItemService
{
    public Task<Response<List<TodoItemDto>>> GetTodoItemAsync(TodoItemFilter filter);
    public Task<Response<TodoItemDto>> AddTodoItemAsync(TodoItemAddDto todoItemAddDto);
    public Task<Response<TodoItemDto>> UpdateTodoItemAsync(TodoItemDto todoItemDto);
    public Task<Response<string>> DeleteTodoItemAsync(int id);

    public Task<Response<TodoItemDto>> GetTodoItemByIdAsync(int id);
    //Making true structure by  Abubakr
}