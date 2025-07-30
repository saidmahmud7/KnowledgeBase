using Domain.Dto.IssueDto;

namespace Domain.Dto.CategoryDto;

public class GetCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } 
    public int SubDepartmentId { get; set; }
    public List<GetIssuesDto>? Issues { get; set; } 
}