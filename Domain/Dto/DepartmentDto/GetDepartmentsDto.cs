using System.ComponentModel.DataAnnotations;
using Domain.Dto.IssueDto;
using Domain.Dto.SubDepartmentDto;
using Domain.Entities;

namespace Domain.Dto.DepartmentDto;

public class GetDepartmentsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<GetSubDepartmentDto>? SubDepartments { get; set; } = new();
}