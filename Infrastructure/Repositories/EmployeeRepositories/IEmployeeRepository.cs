using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Repositories.EmployeeRepositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAll(EmployeeFilter filter);
    Task<Employee?> GetEmployee(Expression<Func<Employee, bool>>? filter = null);
    Task<int> CreateEmployee(Employee request);
    Task<int> UpdateEmployee(Employee request);
    Task<int> DeleteEmployee(Employee request);
}