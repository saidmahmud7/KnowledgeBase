namespace Domain.Dto.CategoryDto;

public class UpdateCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SubDepartmentId { get; set; }
}