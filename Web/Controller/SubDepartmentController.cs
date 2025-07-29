using Domain.Dto.SubDepartmentDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class SubDepartmentController(ISubDepartmentService service) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<PaginationResponse<List<GetSubDepartmentDto>>> GetAll([FromQuery] SubDepartmentFilter filter) =>
        await service.GetAllSubDepartmentAsync(filter);

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ApiResponse<GetSubDepartmentDto>> GetById(int id) => await service.GetByIdAsync(id);

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Create([FromBody] AddSubDepartmentDto request) =>
        await service.CreateAsync(request);

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Update([FromRoute] int id, [FromBody] UpdateSubDepartmentDto request) =>
        await service.UpdateAsync(id, request);

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Delete([FromRoute] int id) => await service.DeleteAsync(id);
}