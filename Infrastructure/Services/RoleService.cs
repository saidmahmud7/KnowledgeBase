using Domain.Dto.RoleDto;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RoleService(RoleManager<IdentityRole> roleManager) : IRoleService
{
    public async Task<ApiResponse<List<Role>>> GetAllRolesAsync()
    {
        var roles = await roleManager.Roles.Select(r => new Role
        {
            Id = r.Id,
            Name = r.Name
        }).ToListAsync();
        return new ApiResponse<List<Role>>(roles);
    }
}