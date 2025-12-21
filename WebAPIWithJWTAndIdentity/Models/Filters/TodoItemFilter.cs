using WebAPIWithJWTAndIdentity.Models.Entity;

namespace WebAPIWithJWTAndIdentity.Models.Filters;

public class TodoItemFilter
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public bool? IsCompleted { get; set; }
    public string? UserId { get; set; } 
}