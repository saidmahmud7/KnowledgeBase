using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Solution
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [NotMapped]
    public IFormFile? ProfileImage { get; set; }
    public string? ProfileImagePath { get; set; }  
    //navigation
    public int IssueId { get; set; }
    public Issue Issue { get; set; }
}