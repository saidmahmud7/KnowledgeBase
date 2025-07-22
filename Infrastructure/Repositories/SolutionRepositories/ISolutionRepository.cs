using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Repositories.SolutionRepositories;

public interface ISolutionRepository
{
    Task<List<Solution>> GetAll(SolutionFilter filter, int? departmentId);
    Task<Solution> GetSolution(Expression<Func<Solution, bool>>? filter = null);
    Task<int> CreateSolution(Solution request);
    Task<int>UpdateSolution(Solution request);
    Task<int>DeleteSolution(Solution request);
}