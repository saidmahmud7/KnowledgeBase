
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

public class User : IdentityUser
{
    public Employee? Employee { get; set; }
}