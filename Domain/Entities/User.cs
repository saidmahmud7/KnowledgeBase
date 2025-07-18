
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;

public class User : IdentityUser
{
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
}