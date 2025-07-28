using Domain.Dto.RoleDto;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;


public interface IRoleService
{
    Task<ApiResponse<List<Role>>> GetAllRolesAsync();
}