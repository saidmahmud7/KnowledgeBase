using Domain.Dto.SolutionDto;
using Domain.Filter;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface ISolutionService
{
    Task<PaginationResponse<List<GetSolutionsDto>>> GetAllSolutionAsync(SolutionFilter filter,int? departmentId);
    Task<ApiResponse<GetSolutionsDto>> GetByIdAsync(int id);
    Task<ApiResponse<string>> CreateAsync(AddSolutionDto request);
    Task<ApiResponse<string>> UpdateAsync(int id,UpdateSolutionDto request);
    Task<ApiResponse<string>> DeleteAsync(int id);
}