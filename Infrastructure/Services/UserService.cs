using System.Net;
using Domain.Dto.UserDto;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.UserRepositories;
using Infrastructure.Response;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(IUserRepository repository , UserManager<User> userManager) : IUserService
{
    public async Task<PaginationResponse<List<GetUserDto>>> GetAllUserAsync(UserFilter filter)
    {
        var users = await repository.GetAll(filter);
        var totalRecords = users.Count;
        var data = users
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        var result = data.Select(u => new GetUserDto()
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Password = u.PasswordHash,
            CreatedAt = u.CreatedAt,
        }).ToList();
        return new PaginationResponse<List<GetUserDto>>(result, totalRecords, filter.PageNumber,
            filter.PageSize);
    }

    public async Task<ApiResponse<string>> UpdateAsync(string id, UpdateUserDto request)
    {
        var user = await repository.GetUser(d => d.Id == id);
        if (user == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "User not found");
        }
        var userWithSameName = await userManager.FindByNameAsync(request.UserName);
        if (userWithSameName != null && userWithSameName.Id != user.Id)
            return new ApiResponse<string>(HttpStatusCode.BadRequest, "Username already taken");
        
        user.Id = request.Id;
        user.UserName = request.UserName;
        user.Email = request.Email;
        user.CreatedAt = DateTime.UtcNow;

      
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await userManager.ResetPasswordAsync(user, token, request.Password);
            if (!passwordResult.Succeeded)
            {
                var errorMessage = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                return new ApiResponse<string>(HttpStatusCode.BadRequest, errorMessage);
            }
        }

        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
            return new ApiResponse<string>("User updated successfully");

        var updateErrors = string.Join(", ", result.Errors.Select(e => e.Description));
        return new ApiResponse<string>(HttpStatusCode.BadRequest, updateErrors);
    }

    public async Task<ApiResponse<string>> DeleteAsync(string id)
    {
        var user = await repository.GetUser(d => d.Id == id);
        if (user == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "User not found");
        }

        var result = await repository.DeleteUser(user);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}