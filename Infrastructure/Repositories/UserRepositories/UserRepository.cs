using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.UserRepositories;

public class UserRepository(DataContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<List<User>> GetAll(UserFilter filter)
    {
        var query = context.Users.OrderByDescending(c=> c.CreatedAt).AsQueryable();

        var users = await query.ToListAsync();
        return users;
    }

    public async Task<User?> GetUser(Expression<Func<User, bool>>? filter = null)
    {
        var query = context.Users.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> UpdateUser(User request)
    {
        try
        {
            context.Users.Update(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message,"Ошибка при обновлении User-а");
            return 0;
        }
    }

    public async Task<int> DeleteUser(User request)
    {
        try
        {
            context.Users.Remove(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message,"Ошибка при удалении User-a");
            return 0;
        }
    }
}