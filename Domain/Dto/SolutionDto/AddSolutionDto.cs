using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Dto.SolutionDto;

public class AddSolutionDto
{
    public string Description { get; set; }
    public IFormFile? ProfileImage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //navigation
    public int IssueId { get; set; }
    public int EmployeeId { get; set; } 

}