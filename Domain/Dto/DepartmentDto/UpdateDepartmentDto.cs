using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.Dto.DepartmentDto;

public class UpdateDepartmentDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}