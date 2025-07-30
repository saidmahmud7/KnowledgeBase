using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.UserDto;

public class GetUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; } 

}