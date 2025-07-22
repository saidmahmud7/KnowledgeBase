using Domain.Dto.EmployeeDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;
[ApiController]
[Route("api/[controller]")]
public class EmployeeController(IEmployeeService service) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResponse<List<GetEmployeeDto>>> GetAll([FromQuery] EmployeeFilter filter) =>
        await service.GetAllEmployeeAsync(filter);

    [HttpGet("{id}")]
    public async Task<ApiResponse<GetEmployeeDto>> GetById(int id) => await service.GetByIdAsync(id);

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Create([FromBody] AddEmployeeDto request) =>
        await service.CreateAsync(request);

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Update([FromRoute] int id, [FromBody] UpdateEmployeeDto request) =>
        await service.UpdateAsync(id, request);

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Delete([FromRoute] int id) => await service.DeleteAsync(id);
}