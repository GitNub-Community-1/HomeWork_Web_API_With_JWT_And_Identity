using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class RegisterDto
{
    public string Username { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}