using Domain.Dto.CategoryDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<PaginationResponse<List<GetCategoryDto>>> GetAll([FromQuery] CategoryFilter filter) =>
        await service.GetAllCategoryAsync(filter);

    [HttpGet("{id}")]
    public async Task<ApiResponse<GetCategoryDto>> GetById(int id) => await service.GetByIdAsync(id);

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Create([FromBody] AddCategoryDto request) =>
        await service.CreateAsync(request);

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Update([FromRoute] int id, [FromBody] UpdateCategoryDto request) =>
        await service.UpdateAsync(id, request);

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ApiResponse<string>> Delete([FromRoute] int id) => await service.DeleteAsync(id);
}