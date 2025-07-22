using System.Net;
using Domain.Dto.DepartmentDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(IDepartmentService service) : ControllerBase
{
    private int? GetDepartmentIdFromToken()
    {
        var claim = User.FindFirst("DepartmentId");
        return int.TryParse(claim?.Value, out var id) ? id : null;
    }

    [HttpGet]
    [Authorize]
    public async Task<PaginationResponse<List<GetDepartmentsDto>>> GetAll([FromQuery] DepartmentFilter filter)
    {
        var departmentId = GetDepartmentIdFromToken();
        return await service.GetAllDepartmentsAsync(filter, departmentId);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ApiResponse<GetDepartmentsDto>> GetById(int id) => await service.GetByIdAsync(id);

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Create([FromBody] AddDepartmentDto request) =>
        await service.CreateAsync(request);

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Update([FromRoute] int id, [FromBody] UpdateDepartmentDto request) =>
        await service.UpdateAsync(id, request);

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Delete([FromRoute] int id) => await service.DeleteAsync(id);
}