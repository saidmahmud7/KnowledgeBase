using Domain.Dto.SubDepartmentDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;
[ApiController]
[Route("api/[controller]")]
public class SubDepartmentController(ISubDepartmentService service) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResponse<List<GetSubDepartmentDto>>> GetAll([FromQuery]SubDepartmentFilter filter) => await service.GetAllSubDepartmentAsync(filter);
    
    [HttpGet("{id}")]
    public async Task<ApiResponse<GetSubDepartmentDto>> GetById(int id) => await service.GetByIdAsync(id);
    
    [HttpPost]
    public async Task<ApiResponse<string>> Create([FromForm]AddSubDepartmentDto request) => await service.CreateAsync(request);
    
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> Update([FromRoute]int id,[FromBody]UpdateSubDepartmentDto request) => await service.UpdateAsync(id,request);
    
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> Delete([FromRoute]int id) => await service.DeleteAsync(id);
}