using Domain.Dto.DepartmentDto;
using Domain.Filter;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface IDepartmentService
{
    Task<PaginationResponse<List<GetDepartmentsDto>>> GetAllDepartmentsAsync(DepartmentFilter filter, int? departmentId);
    Task<ApiResponse<GetDepartmentsDto>> GetByIdAsync(int id);
    Task<ApiResponse<string>> CreateAsync(AddDepartmentDto request);
    Task<ApiResponse<string>> UpdateAsync(int id,UpdateDepartmentDto request);
    Task<ApiResponse<string>> DeleteAsync(int id);
}