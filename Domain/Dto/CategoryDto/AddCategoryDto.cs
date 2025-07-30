namespace Domain.Dto.CategoryDto;

public class AddCategoryDto
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } 
    public int SubDepartmentId { get; set; }
}