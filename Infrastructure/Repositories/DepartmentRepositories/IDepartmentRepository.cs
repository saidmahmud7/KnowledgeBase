using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Repositories.DepartmentRepositories;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAll(DepartmentFilter filter);
    Task<Department?> GetDepartment(Expression<Func<Department, bool>>? filter = null);
    Task<int> CreateDepartment(Department request);
    Task<int> UpdateDepartment(Department request);
    Task<int> DeleteDepartment(Department request);
}