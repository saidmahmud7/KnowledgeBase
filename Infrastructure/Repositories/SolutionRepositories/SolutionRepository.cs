using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.SolutionRepositories;

public class SolutionRepository(DataContext context, ILogger<SolutionRepository> logger) : ISolutionRepository
{
    public async Task<List<Solution>> GetAll(SolutionFilter filter)
    {
        var query = context.Solutions.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.Description))
            query = query.Where(e => e.Description.ToLower().Trim().Contains(filter.Description.ToLower().Trim()));
        
        var solutions = await query.ToListAsync();
        return solutions;
    }

    public async Task<Solution> GetSolution(Expression<Func<Solution, bool>>? filter = null)
    {
        var query = context.Solutions.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CreateSolution(Solution request)
    {
        try
        {
            await context.Solutions.AddAsync(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Ошибка при добавлении: {SolutionName}",request.Description);
            return 0;
        }
    }

    public async Task<int> UpdateSolution(Solution request)
    {
        try
        {
            context.Solutions.Update(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Ошибка при обновлении");
            return 0;
        }
    }

    public async Task<int> DeleteSolution(Solution request)
    {
        try
        {
            context.Solutions.Remove(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e,"Ошибка при удалении");
            return 0;
        }
    }
}