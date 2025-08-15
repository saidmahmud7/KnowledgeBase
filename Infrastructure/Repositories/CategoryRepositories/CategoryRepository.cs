using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.CategoryRepositories;

public class CategoryRepository(DataContext context, ILogger<CategoryRepository> logger) : ICategoryRepository
{
    public async Task<List<Category>> GetAll(CategoryFilter filter)
    {
        var query = context.Categories.Include(i => i.Issues).OrderByDescending(c=> c.CreatedAt).AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(e => e.Name.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));

        var categories = await query.ToListAsync();
        return categories;
    }

    public async Task<Category?> GetCategory(Expression<Func<Category, bool>>? filter = null)
    {
        var query = context.Categories.Include(i => i.Issues).AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CreateCategory(Category request)
    {
        try
        {
            await context.Categories.AddAsync(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Ошибка при добавлении категории: {CategoryName}",request.Name);
            return 0;
        }
    }

    public async Task<int> UpdateCategory(Category request)
    {
        try
        {
            context.Categories.Update(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Ошибка при обновлении категории");
            return 0;
        }
    }

    public async Task<int> DeleteCategory(Category request)
    {
        try
        {
            context.Categories.Remove(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Ошибка при удалении категории");
            return 0;
        }
    }
}