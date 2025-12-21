using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIWithJWTAndIdentity.Data;
using WebAPIWithJWTAndIdentity.Models.Dtos;
using WebAPIWithJWTAndIdentity.Models.Entity;
using WebAPIWithJWTAndIdentity.Models.Filters;
using WebAPIWithJWTAndIdentity.Services.Interfaces;
using WebAPIWithJWTAndIdentity.Response;
namespace WebAPIWithJWTAndIdentity.Services;

public class TodoItemService(ApplicationDbContext context, IMapper mapper) : ITodoItemService
{
    public async Task<Response<List<TodoItemDto>>> GetTodoItemAsync(TodoItemFilter filter)
    {
        /*try
        {*/
            var query = context.TodoItems
                .AsQueryable();
            
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id.Value);
            }
            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => x.Title.Contains(filter.Title));
            }
            if (filter.IsCompleted != null)
            {
                query = query.Where(x => x.IsCompleted != null && x.IsCompleted == filter.IsCompleted);
            }

            var todoitem = await query.ToListAsync();
            var result = mapper.Map<List<TodoItemDto>>(todoitem);
            return new Response<List<TodoItemDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Authors retrieved successfully!",
                Data = result
            };
        /*}*/
        /*catch (Exception ex)
        {
            return new Response<List<TodoItemDto>>(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
        }*/
    }

    public async Task<Response<TodoItemDto>> AddTodoItemAsync(TodoItemAddDto todoItemAddDto)
    {
        /*try
        {*/
            var todoItem = mapper.Map<TodoItem>(todoItemAddDto);
            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();
            var result = mapper.Map<TodoItemDto>(todoItem);
            return new Response<TodoItemDto>(HttpStatusCode.Created, "TodoItem created successfully!", result);
        /*}*/
        /*catch (Exception ex)
        {
            return new Response<TodoItemDto>(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
        }*/
    }

    public async Task<Response<TodoItemDto>> UpdateTodoItemAsync(TodoItemDto todoItemDto)
    {
        /*try
        {*/
            var check = await context.TodoItems.FindAsync(todoItemDto.Id);
            if (check == null)
                return new Response<TodoItemDto>(HttpStatusCode.NotFound, "TodoItem not found");
            
            check.Title = todoItemDto.Title;
            check.IsCompleted = todoItemDto.IsCompleted;
            check.UserId = todoItemDto.UserId;
            context.TodoItems.Update(check);
            await context.SaveChangesAsync();
            var result = mapper.Map<TodoItemDto>(check);
            return new Response<TodoItemDto>(HttpStatusCode.OK, "TodoItem updated successfully!", result);
        /*}*/
        /*catch (Exception ex)
        {
            return new Response<TodoItemDto>(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
        }*/
    }

    public async Task<Response<string>> DeleteTodoItemAsync(int id)
    {
        /*try
        {*/
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null)
            return new Response<string>(HttpStatusCode.NotFound, "TodoItem not found");

        context.TodoItems.Remove(todoItem);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "TodoItem deleted successfully!");

        /*catch (Exception ex)
            {
                return new Response<string>(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }*/
    }

    public async Task<Response<TodoItemDto>> GetTodoItemByIdAsync(int id)
    {
        /*try
        {*/
            var todoItem = await context.TodoItems.FirstOrDefaultAsync(a => a.Id == id);
            if (todoItem == null)
                return new Response<TodoItemDto>(HttpStatusCode.NotFound, "TodoItem not found");
            
            var result = mapper.Map<TodoItemDto>(todoItem);
            return new Response<TodoItemDto>(HttpStatusCode.OK, "Author retrieved successfully!", result);
        
        /*catch (Exception ex)
        {
            return new Response<TodoItemDto>(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
        }*/
    }
}