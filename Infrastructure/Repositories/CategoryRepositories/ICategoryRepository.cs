using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Repositories.CategoryRepositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAll(CategoryFilter filter);
    Task<Category?> GetCategory(Expression<Func<Category, bool>>? filter = null);
    Task<int> CreateCategory(Category request);
    Task<int> UpdateCategory(Category request);
    Task<int> DeleteCategory(Category request);
}