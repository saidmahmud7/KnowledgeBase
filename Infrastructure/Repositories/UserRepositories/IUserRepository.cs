using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Repositories.UserRepositories;

public interface IUserRepository
{
    Task<List<User>> GetAll(UserFilter filter);
    Task<User> GetUser(Expression<Func<User, bool>>? filter = null);
    Task<int>UpdateUser(User request);
    Task<int>DeleteUser(User request);
}