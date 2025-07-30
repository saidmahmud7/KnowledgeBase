namespace Domain.Dto.SubDepartmentDto;

public class UpdateSubDepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } 

    public int DepartmentId { get; set; }
}