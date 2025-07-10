using Microsoft.AspNetCore.Http;

namespace Domain.Dto.IssueDto;

public class UpdateIssueDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IFormFile? ProfileImage { get; set; }
    public int CategoryId { get; set; }
}