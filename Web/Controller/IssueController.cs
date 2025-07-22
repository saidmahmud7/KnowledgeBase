using Domain.Dto.DepartmentDto;
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
public class IssueController(IIssueService service) : ControllerBase
{

    [HttpGet]
    [Authorize]
    public async Task<PaginationResponse<List<GetIssuesDto>>> GetAll([FromQuery] IssueFilter filter)
    {
        return await service.GetAllIssueAsync(filter);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<GetIssuesDto>> GetById(int id) => await service.GetByIdAsync(id);

    [HttpPost]
    public async Task<ApiResponse<string>> Create([FromForm] AddIssueDto request) => await service.CreateAsync(request);

    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> Update([FromRoute] int id, [FromForm] UpdateIssueDto request) =>
        await service.UpdateAsync(id, request);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> Delete([FromRoute] int id) => await service.DeleteAsync(id);
}