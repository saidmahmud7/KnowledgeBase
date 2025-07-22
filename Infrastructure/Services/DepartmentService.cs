using System.Net;
using Domain.Dto.DepartmentDto;
using Domain.Dto.IssueDto;
using Domain.Dto.SubDepartmentDto;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.DepartmentRepositories;
using Infrastructure.Response;

namespace Infrastructure.Services;

public class DepartmentService(IDepartmentRepository repository) : IDepartmentService
{
    public async Task<PaginationResponse<List<GetDepartmentsDto>>> GetAllDepartmentsAsync(DepartmentFilter filter, int? departmentId)
    {
        var departments = await repository.GetAll(filter,departmentId);
        var totalRecords = departments.Count;
        var data = departments
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        var result = data.Select(d => new GetDepartmentsDto()
        {
            Id = d.Id,
            Name = d.Name,
            SubDepartments = d.SubDepartments?.Select(i => new GetSubDepartmentDto()
            {
                Id = i.Id,
                Name = i.Name,
                DepartmentId = i.DepartmentId
            }).ToList()
        }).ToList();

        return new PaginationResponse<List<GetDepartmentsDto>>(result, totalRecords, filter.PageNumber,
            filter.PageSize);
    }

    public async Task<ApiResponse<GetDepartmentsDto>> GetByIdAsync(int id)
    {
        var department = await repository.GetDepartment(d => d.Id == id);
        if (department == null)
        {
            return new ApiResponse<GetDepartmentsDto>(HttpStatusCode.NotFound, "Department Not Found");
        }

        var result = new GetDepartmentsDto()
        {
            Id = department.Id,
            Name = department.Name,
            SubDepartments = department.SubDepartments?.Select(i => new GetSubDepartmentDto()
            {
                Id = i.Id,
                Name = i.Name,
                DepartmentId = i.DepartmentId,
            }).ToList()
        };
        return new ApiResponse<GetDepartmentsDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddDepartmentDto request)
    {
        var department = new Department()
        {
            Name = request.Name,
        };
        var result = await repository.CreateDepartment(department);

        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateDepartmentDto request)
    {
        var department = await repository.GetDepartment(d => d.Id == id);
        if (department == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Department Not Found");
        }


        department.Name = request.Name;

        var result = await repository.UpdateDepartment(department);

        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var department = await repository.GetDepartment(d => d.Id == id);
        if (department == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Department Not Found");
        }

        var result = await repository.DeleteDepartment(department);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}