namespace WebAPIWithJWTAndIdentity.Models.Dtos;

public class TodoItemAddDto
{
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public string UserId { get; set; } = null!;
}