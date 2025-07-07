using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Dto.IssueDto;

public class AddIssueDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IFormFile? ProfileImage { get; set; }
    public int DepartmentId { get; set; }
}