using Domain.Dto.SubDepartmentDto;
using Domain.Filter;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface ISubDepartmentService
{
    Task<PaginationResponse<List<GetSubDepartmentDto>>> GetAllSubDepartmentAsync(SubDepartmentFilter filter);
    Task<ApiResponse<GetSubDepartmentDto>> GetByIdAsync(int id);
    Task<ApiResponse<string>> CreateAsync(AddSubDepartmentDto request);
    Task<ApiResponse<string>> UpdateAsync(int id,UpdateSubDepartmentDto request);
    Task<ApiResponse<string>> DeleteAsync(int id);
}