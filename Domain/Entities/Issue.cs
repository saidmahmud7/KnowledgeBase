using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Issue
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [NotMapped]
    public IFormFile? ProfileImage { get; set; }
    public string? ProfileImagePath { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int? EmployeeId { get; set; } 
    public Employee Employee { get; set; }
    public List<Solution>? Solutions { get; set; }
}