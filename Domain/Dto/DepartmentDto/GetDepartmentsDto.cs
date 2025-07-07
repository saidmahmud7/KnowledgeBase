using System.ComponentModel.DataAnnotations;
using Domain.Dto.IssueDto;
using Domain.Entities;

namespace Domain.Dto.DepartmentDto;

public class GetDepartmentsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<GetIssuesDto> Issues { get; set; } = new();
}