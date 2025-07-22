
using Domain.Dto.EmployeeDto;
using Domain.Filter;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface IEmployeeService
{
    Task<PaginationResponse<List<GetEmployeeDto>>> GetAllEmployeeAsync(EmployeeFilter filter);
    Task<ApiResponse<GetEmployeeDto>> GetByIdAsync(int id);
    Task<ApiResponse<string>> CreateAsync(AddEmployeeDto request);
    Task<ApiResponse<string>> UpdateAsync(int id,UpdateEmployeeDto request);
    Task<ApiResponse<string>> DeleteAsync(int id);
}