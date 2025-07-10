using Domain.Dto.CategoryDto;
using Domain.Filter;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface ICategoryService
{
    Task<PaginationResponse<List<GetCategoryDto>>> GetAllCategoryAsync(CategoryFilter filter);
    Task<ApiResponse<GetCategoryDto>> GetByIdAsync(int id);
    Task<ApiResponse<string>> CreateAsync(AddCategoryDto request);
    Task<ApiResponse<string>> UpdateAsync(int id,UpdateCategoryDto request);
    Task<ApiResponse<string>> DeleteAsync(int id);
}