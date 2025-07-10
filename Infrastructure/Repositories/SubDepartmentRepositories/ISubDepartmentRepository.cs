using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Repositories.SubDepartmentRepositories;

public interface ISubDepartmentRepository
{
    Task<List<SubDepartment?>> GetAll(SubDepartmentFilter filter);
    Task<SubDepartment?> GetSubDepartment(Expression<Func<SubDepartment, bool>>? filter = null);
    Task<int> CreateSubDepartment(SubDepartment request);
    Task<int>UpdateSubDepartment(SubDepartment request);
    Task<int>DeleteSubDepartment(SubDepartment request);
}