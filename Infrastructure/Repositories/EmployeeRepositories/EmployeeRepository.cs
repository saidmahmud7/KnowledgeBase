using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.EmployeeRepositories;

public class EmployeeRepository(DataContext context, ILogger<EmployeeRepository> logger) : IEmployeeRepository
{
    public async Task<List<Employee>> GetAll(EmployeeFilter filter)
    {
        var query = context.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(filter.FullName))
            query = query.Where(e => e.FullName.ToLower().Trim().Contains(filter.FullName.ToLower().Trim()));

        var employee = await query.ToListAsync();
        return employee;
    }

    public async Task<Employee?> GetEmployee(Expression<Func<Employee, bool>>? filter = null)
    {
        var query = context.Employees.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CreateEmployee(Employee request)
    {
        try
        {
            await context.Employees.AddAsync(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> UpdateEmployee(Employee request)
    {
        try
        {
            context.Employees.Update(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> DeleteEmployee(Employee request)
    {
        try
        {
            context.Employees.Remove(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }
}