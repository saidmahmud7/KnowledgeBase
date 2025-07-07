using Domain.Dto.SolutionDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;
[ApiController]
[Route("api/[controller]")]
public class SolutionController(ISolutionService service) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResponse<List<GetSolutionsDto>>> GetAll([FromQuery]SolutionFilter filter) => await service.GetAllSolutionAsync(filter);
    
    [HttpGet("{id}")]
    public async Task<ApiResponse<GetSolutionsDto>> GetById(int id) => await service.GetByIdAsync(id);
    
    [HttpPost]
    public async Task<ApiResponse<string>> Create([FromForm]AddSolutionDto request) => await service.CreateAsync(request);
    
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> Update([FromRoute]int id,[FromBody]UpdateSolutionDto request) => await service.UpdateAsync(id,request);
    
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> Delete([FromRoute]int id) => await service.DeleteAsync(id); 
}