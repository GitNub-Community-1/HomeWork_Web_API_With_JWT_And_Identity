namespace WebAPIWithJWTAndIdentity.Models.Entity;

public class TodoItem : BaseEntity
{
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public string UserId { get; set; } = null!;
}