using System.Net;
using Domain.Dto.CategoryDto;
using Domain.Dto.IssueDto;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.CategoryRepositories;
using Infrastructure.Response;

namespace Infrastructure.Services;

public class CategoryService(ICategoryRepository repository) : ICategoryService
{
    public async Task<PaginationResponse<List<GetCategoryDto>>> GetAllCategoryAsync(CategoryFilter filter)
    {
        var category = await repository.GetAll(filter);
        var totalRecords = category.Count;
        var data = category
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        var result = data.Select(c => new GetCategoryDto()
        {
            Id = c.Id,
            Name = c.Name,
            CreatedAt = c.CreatedAt,
            SubDepartmentId = c.SubDepartmentId,
            Issues = c.Issues?.Select(i => new GetIssuesDto()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                CreatedAt = i.CreatedAt,
                ProfileImagePath = i.ProfileImagePath,
                CategoryId = i.CategoryId,
            }).ToList()
        }).ToList();
        return new PaginationResponse<List<GetCategoryDto>>(result, totalRecords, filter.PageNumber,
            filter.PageSize);
    }

    public async Task<ApiResponse<GetCategoryDto>> GetByIdAsync(int id)
    {
        var category = await repository.GetCategory(d => d.Id == id);
        if (category == null)
        {
            return new ApiResponse<GetCategoryDto>(HttpStatusCode.NotFound, "Category Not Found");
        }

        var result = new GetCategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt,
            SubDepartmentId = category.SubDepartmentId,
            Issues = category.Issues?.Select(i => new GetIssuesDto()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                CreatedAt = i.CreatedAt,
                ProfileImagePath = i.ProfileImagePath,
                CategoryId = i.CategoryId,
            }).ToList()
        };
        return new ApiResponse<GetCategoryDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddCategoryDto request)
    {
        var category = new Category()
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            SubDepartmentId = request.SubDepartmentId,
        };
        var result = await repository.CreateCategory(category);
        
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK,"Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateCategoryDto request)
    {
        var category = await repository.GetCategory(d => d.Id == id);
        if (category == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Category Not Found");
        }
       
        category.Name = request.Name;
        category.CreatedAt = DateTime.UtcNow;
        category.SubDepartmentId = request.SubDepartmentId;
        
        var result = await repository.UpdateCategory(category);

        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK,"Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var category = await repository.GetCategory(d => d.Id == id);
        if (category == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Category Not Found");
        }
        
        var result = await repository.DeleteCategory(category);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK,"Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}