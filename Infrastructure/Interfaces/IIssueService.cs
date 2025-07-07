using Domain.Dto.IssueDto;
using Domain.Filter;
using Infrastructure.Response;

namespace Infrastructure.Interfaces;

public interface IIssueService
{
    Task<PaginationResponse<List<GetIssuesDto>>> GetAllIssueAsync(IssueFilter filter);
    Task<ApiResponse<GetIssuesDto>> GetByIdAsync(int id);
    Task<ApiResponse<string>> CreateAsync(AddIssueDto request);
    Task<ApiResponse<string>> UpdateAsync(int id,UpdateIssueDto request);
    Task<ApiResponse<string>> DeleteAsync(int id);
}