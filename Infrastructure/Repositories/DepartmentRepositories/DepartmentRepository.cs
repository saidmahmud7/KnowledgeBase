using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.DepartmentRepositories;

public class DepartmentRepository(DataContext context, ILogger<DepartmentRepository> logger) : IDepartmentRepository
{
    public async Task<List<Department>> GetAll(DepartmentFilter filter)
    {
        var query = context.Departments.Include(i => i.Issues).AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(e => e.Name.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));
        
        var departments = await query.ToListAsync();
        return departments;
    }

    public async Task<Department?> GetDepartment(Expression<Func<Department, bool>>? filter = null)
    {
        var query = context.Departments.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CreateDepartment(Department request)
    {
        try
        {
            await context.Departments.AddAsync(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> UpdateDepartment(Department request)
    {
        try
        {
            context.Departments.Update(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> DeleteDepartment(Department request)
    {
        try
        {
            context.Departments.Remove(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }
}