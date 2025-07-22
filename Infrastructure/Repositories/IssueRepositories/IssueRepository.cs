using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.IssueRepositories;

public class IssueRepository(DataContext context, ILogger<IssueRepository> logger) : IIssueRepository
{
    public async Task<List<Issue>> GetAll(IssueFilter filter, int? departmentId = null)
    {
        var query = context.Issues.Include(s=>s.Solutions).AsQueryable();
        if (!string.IsNullOrEmpty(filter.Title))
            query = query.Where(e => e.Title.ToLower().Trim().Contains(filter.Title.ToLower().Trim()));
        if (!string.IsNullOrEmpty(filter.Description))
            query = query.Where(e => e.Description.ToLower().Trim().Contains(filter.Description.ToLower().Trim()));
        if (departmentId.HasValue)
            query = query.Where(x => x.Employee != null && x.Employee.DepartmentId == departmentId); 
        
        var issues = await query.ToListAsync();
        return issues;
    }

    public async Task<Issue> GetIssue(Expression<Func<Issue, bool>>? filter = null)
    {
        var query = context.Issues.Include(s=> s.Solutions).AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CreateIssue(Issue request)
    {
        try
        {
            await context.Issues.AddAsync(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> UpdateIssue(Issue request)
    {
        try
        {
            context.Issues.Update(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }

    public async Task<int> DeleteIssue(Issue request)
    {
        try
        {
            context.Issues.Remove(request);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return 0;
        }
    }
}