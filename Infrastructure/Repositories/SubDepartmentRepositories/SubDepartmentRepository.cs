using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.SubDepartmentRepositories;

public class SubDepartmentRepository(DataContext context, ILogger<SubDepartmentRepository> logger)
    : ISubDepartmentRepository
{
    public async Task<List<SubDepartment?>> GetAll(SubDepartmentFilter filter)
    {
        var query = context.SubDepartments.Include(c => c.Categories).AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(e => e.Name.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));

        var subDepartments = await query.ToListAsync();
        return subDepartments;
    }

    public async Task<SubDepartment?> GetSubDepartment(Expression<Func<SubDepartment, bool>>? filter = null)
    {
        var query = context.SubDepartments.Include(d => d.Categories).AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CreateSubDepartment(SubDepartment request)
    {
        try
        {
            await context.SubDepartments.AddAsync(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> UpdateSubDepartment(SubDepartment request)
    {
        try
        {
            context.SubDepartments.Update(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> DeleteSubDepartment(SubDepartment request)
    {
        try
        {
            context.SubDepartments.Remove(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }
}