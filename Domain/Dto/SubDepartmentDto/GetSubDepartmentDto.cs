using Domain.Dto.CategoryDto;

namespace Domain.Dto.SubDepartmentDto;

public class GetSubDepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } 

    public int DepartmentId { get; set; }
    public List<GetCategoryDto>? Categories { get; set; }
}