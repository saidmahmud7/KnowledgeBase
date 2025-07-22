using Domain.Dto.IssueDto;
using Domain.Dto.SolutionDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class SolutionController(ISolutionService service) : ControllerBase
{
    private int? GetDepartmentIdFromToken()
    {
        var claim = User.FindFirst("DepartmentId");
        return int.TryParse(claim?.Value, out var id) ? id : null;
    }

    [HttpGet]
    [Authorize]
    public async Task<PaginationResponse<List<GetSolutionsDto>>> GetAll([FromQuery] SolutionFilter filter)
    {
        var departmentId = GetDepartmentIdFromToken();
        return await service.GetAllSolutionAsync(filter, departmentId);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<GetSolutionsDto>> GetById(int id) => await service.GetByIdAsync(id);

    [HttpPost]
    public async Task<ApiResponse<string>> Create([FromForm] AddSolutionDto request) =>
        await service.CreateAsync(request);

    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> Update([FromRoute] int id, [FromForm] UpdateSolutionDto request) =>
        await service.UpdateAsync(id, request);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> Delete([FromRoute] int id) => await service.DeleteAsync(id);
}