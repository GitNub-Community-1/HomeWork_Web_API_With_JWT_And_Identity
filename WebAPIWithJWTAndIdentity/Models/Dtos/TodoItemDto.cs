using WebAPIWithJWTAndIdentity.Models.Entity;

namespace WebAPIWithJWTAndIdentity.Models.Dtos;

public class TodoItemDto : BaseEntity
{
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public string UserId { get; set; } = null!;
}