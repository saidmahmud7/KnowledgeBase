namespace Domain.Dto.SubDepartmentDto;

public class AddSubDepartmentDto
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } 

    public int DepartmentId { get; set; }
}