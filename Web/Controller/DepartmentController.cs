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
    [Authorize]
    [HttpGet]
    public async Task<PaginationResponse<List<GetDepartmentsDto>>> GetAll([FromQuery] DepartmentFilter filter)
    {
        // Извлекаем departmentId из токена
        var departmentIdClaim = User.FindFirst("DepartmentId")?.Value;

        // Парсим и применяем, если найден
        if (int.TryParse(departmentIdClaim, out var departmentId))
        {
            filter.Id = departmentId;
        }

        return await service.GetAllDepartmentsAsync(filter);
    }
    
    [HttpGet("{id}")]
    public async Task<ApiResponse<GetDepartmentsDto>> GetById(int id) => await service.GetByIdAsync(id);
    
    [HttpPost]
    public async Task<ApiResponse<string>> Create([FromBody]AddDepartmentDto request) => await service.CreateAsync(request);
    
    [HttpPut("{id}")]
    public async Task<ApiResponse<string>> Update([FromRoute]int id,[FromBody]UpdateDepartmentDto request) => await service.UpdateAsync(id,request);
    
    [HttpDelete("{id}")]
     public async Task<ApiResponse<string>> Delete([FromRoute]int id) => await service.DeleteAsync(id);
}                           