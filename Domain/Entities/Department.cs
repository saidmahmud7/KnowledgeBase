using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Department
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public List<Issue> Issues { get; set; } = new();
}