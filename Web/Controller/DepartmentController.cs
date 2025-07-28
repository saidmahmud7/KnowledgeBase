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

    [HttpGet]
    [Authorize(Roles = Roles.User)]
    public async Task<PaginationResponse<List<GetDepartmentsDto>>> GetAll([FromQuery] DepartmentFilter filter)
    {
        
        return await service.GetAllDepartmentsAsync(filter);
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