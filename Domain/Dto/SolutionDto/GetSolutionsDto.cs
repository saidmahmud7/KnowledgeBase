namespace Domain.Dto.SolutionDto;

public class GetSolutionsDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProfileImagePath { get; set; }

    //navigation
    public int IssueId { get; set; }
}