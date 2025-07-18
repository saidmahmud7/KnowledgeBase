using System.Net;
using Domain.Dto.DepartmentDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(IDepartmentService service) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResponse<List<GetDepartmentsDto>>> GetAll([FromQuery] DepartmentFilter filter)
    {
        var departmentIdStr = User.FindFirst("DepartmentId")?.Value;

        if (!int.TryParse(departmentIdStr, out var departmentId))
            return new PaginationResponse<List<GetDepartmentsDto>>(HttpStatusCode.Forbidden,
                "Department ID not found in token.");


        var department = await service.GetAllDepartmentsAsync(filter, departmentId);
        if (department == null)
        {
            return new PaginationResponse<List<GetDepartmentsDto>>(HttpStatusCode.NotFound, "Department not found.");
        }

        return department;
    }


    [HttpGet("{id}")]
    public async Task<ApiResponse<GetDepartmentsDto>> GetById(int id) => await service.GetByIdAsync(id);

    [HttpPost]
    public async Task<ApiResponse<string>> Create([FromBody] AddDepartmentDto request) =>
        await service.CreateAsync(request);

    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> Update([FromRoute] int id, [FromBody] UpdateDepartmentDto request) =>
        await service.UpdateAsync(id, request);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> Delete([FromRoute] int id) => await service.DeleteAsync(id);
}