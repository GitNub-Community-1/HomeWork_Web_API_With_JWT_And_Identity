using System.ComponentModel.DataAnnotations;

namespace WebAPIWithJWTAndIdentity.Models.Entity;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}