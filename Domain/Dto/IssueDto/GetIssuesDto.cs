using Domain.Dto.SolutionDto;
using Domain.Entities;

namespace Domain.Dto.IssueDto;

public class GetIssuesDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProfileImagePath { get; set; }
    public int CategoryId { get; set; }
    public int EmployeeId { get; set; } 
    public List<GetSolutionsDto>? Solutions { get; set; }
}