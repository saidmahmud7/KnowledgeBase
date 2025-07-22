using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filter;

namespace Infrastructure.Repositories.IssueRepositories;

public interface IIssueRepository
{
    Task<List<Issue>> GetAll(IssueFilter filter);
    Task<Issue> GetIssue(Expression<Func<Issue, bool>>? filter = null);
    Task<int> CreateIssue(Issue request);
    Task<int>UpdateIssue(Issue request);
    Task<int>DeleteIssue(Issue request);
}