namespace Domain.Dto.EmployeeDto;

public class UpdateEmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string UserId { get; set; }
    public int DepartmentId { get; set; }
}