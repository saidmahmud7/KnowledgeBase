namespace Domain.Dto.EmployeeDto;

public class AddEmployeeDto
{
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; } 
    public string UserId { get; set; }
    public int DepartmentId { get; set; }
}