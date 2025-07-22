using System.Net;
using Domain.Dto.UserDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.UserRepositories;
using Infrastructure.Response;

namespace Infrastructure.Services;

public class UserService(IUserRepository repository) : IUserService
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

        user.UserName = request.UserName;
        user.Email = request.Email;
        user.PasswordHash = request.Password;
        var result = await repository.UpdateUser(user);

        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
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