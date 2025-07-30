namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int SubDepartmentId { get; set; }
    public SubDepartment SubDepartment { get; set; }
    public List<Issue>? Issues { get; set; } 
}
