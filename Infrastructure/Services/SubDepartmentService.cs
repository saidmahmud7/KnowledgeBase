using System.Net;
using Domain.Dto.CategoryDto;
using Domain.Dto.SubDepartmentDto;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.SubDepartmentRepositories;
using Infrastructure.Response;

namespace Infrastructure.Services;

public class SubDepartmentService(ISubDepartmentRepository repository) : ISubDepartmentService
{
    public async Task<PaginationResponse<List<GetSubDepartmentDto>>> GetAllSubDepartmentAsync(
        SubDepartmentFilter filter)
    {
        var subDepartment = await repository.GetAll(filter);
        var totalRecords = subDepartment.Count;
        var data = subDepartment
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        var result = data.Select(s => new GetSubDepartmentDto()
        {
            Id = s.Id,
            Name = s.Name,
            DepartmentId = s.DepartmentId,
            Categories = s.Categories?.Select(c => new GetCategoryDto()
            {
                Id = c.Id,
                Name = c.Name,
                SubDepartmentId = c.SubDepartmentId
            }).ToList()
        }).ToList();
        return new PaginationResponse<List<GetSubDepartmentDto>>(result, totalRecords, filter.PageNumber,
            filter.PageSize);
    }

    public async Task<ApiResponse<GetSubDepartmentDto>> GetByIdAsync(int id)
    {
        var subDepartment = await repository.GetSubDepartment(d => d.Id == id);
        if (subDepartment == null)
        {
            return new ApiResponse<GetSubDepartmentDto>(HttpStatusCode.NotFound, "SubDepartment Not Found");
        }

        var result = new GetSubDepartmentDto()
        {
            Id = subDepartment.Id,
            Name = subDepartment.Name,
            DepartmentId = subDepartment.DepartmentId,
            Categories = subDepartment.Categories?.Select(c => new GetCategoryDto()
            {
                Id = c.Id,
                Name = c.Name,
                SubDepartmentId = c.SubDepartmentId
            }).ToList()
        };
        return new ApiResponse<GetSubDepartmentDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddSubDepartmentDto request)
    {
        var subDepartment = new SubDepartment()
        {
            Name = request.Name,
            DepartmentId = request.DepartmentId,
        };
        var result = await repository.CreateSubDepartment(subDepartment);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateSubDepartmentDto request)
    {
        var subDepartment = await repository.GetSubDepartment(d => d.Id == id);
        if (subDepartment == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "SubDepartment Not Found");
        }

        subDepartment.Name = request.Name;
        subDepartment.DepartmentId = request.DepartmentId;
        var result = await repository.UpdateSubDepartment(subDepartment);

        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var subDepartment = await repository.GetSubDepartment(d => d.Id == id);
        if (subDepartment == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "SubDepartment Not Found");
        }

        var result = await repository.DeleteSubDepartment(subDepartment);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}