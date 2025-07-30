namespace Domain.Entities;

public class SubDepartment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public List<Category>? Categories { get; set; }
}