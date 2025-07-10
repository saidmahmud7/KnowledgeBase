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
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public List<Solution>? Solutions { get; set; }
}