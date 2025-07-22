using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.Auth;

public class RegisterDto
{
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}
