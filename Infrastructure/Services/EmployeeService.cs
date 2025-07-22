using System.Net;
using Domain.Dto.EmployeeDto;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.EmployeeRepositories;
using Infrastructure.Response;

namespace Infrastructure.Services;

public class EmployeeService(IEmployeeRepository repository) : IEmployeeService
{
    public async Task<PaginationResponse<List<GetEmployeeDto>>> GetAllEmployeeAsync(EmployeeFilter filter)
    {
        var employee = await repository.GetAll(filter);
        var totalRecords = employee.Count;
        var data = employee
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        var result = data.Select(d => new GetEmployeeDto()
        {
            Id = d.Id,
            FullName = d.FullName,
            DepartmentId = d.DepartmentId,
            UserId = d.UserId,
        }).ToList();
        return new PaginationResponse<List<GetEmployeeDto>>(result, totalRecords, filter.PageNumber, filter.PageSize);
    }

    public async Task<ApiResponse<GetEmployeeDto>> GetByIdAsync(int id)
    {
        var employee = await repository.GetEmployee(e => e.Id == id);
        if (employee == null)
        {
            return new ApiResponse<GetEmployeeDto>(HttpStatusCode.NotFound, "Employee not found");
        }

        var result = new GetEmployeeDto()
        {
            Id = employee.Id,
            FullName = employee.FullName,
            DepartmentId = employee.DepartmentId,
            UserId = employee.UserId,
        };
        return new ApiResponse<GetEmployeeDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddEmployeeDto request)
    {
        var employee = new Employee()
        {
            FullName = request.FullName,
            DepartmentId = request.DepartmentId,
            UserId = request.UserId,
        };
        var result = await repository.CreateEmployee(employee);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateEmployeeDto request)
    {
        var employee = await repository.GetEmployee(e => e.Id == id);
        if (employee == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Employee not found");
        }

        employee.FullName = request.FullName;
        employee.DepartmentId = request.DepartmentId;
        employee.UserId = request.UserId;

        var result = await repository.UpdateEmployee(employee);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var employee = await repository.GetEmployee(e => e.Id == id);
        if (employee == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Employee not found");
        }

        var result = await repository.DeleteEmployee(employee);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK, "Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}