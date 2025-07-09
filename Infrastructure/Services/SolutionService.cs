using System.Net;
using Domain.Dto.SolutionDto;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.SolutionRepositories;
using Infrastructure.Response;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

public class SolutionService(ISolutionRepository repository, IWebHostEnvironment _environment) : ISolutionService
{
    public async Task<PaginationResponse<List<GetSolutionsDto>>> GetAllSolutionAsync(SolutionFilter filter)
    {
        var solution = await repository.GetAll(filter);
        var totalRecords = solution.Count;
        var data = solution
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        var result = solution.Select(i => new GetSolutionsDto()
        {
            Id = i.Id,
            Description = i.Description,
            CreatedAt = i.CreatedAt,
            IssueId = i.IssueId,
            ProfileImagePath = i.ProfileImagePath,
        }).ToList();
        return new PaginationResponse<List<GetSolutionsDto>>(result, totalRecords, filter.PageNumber,
            filter.PageSize);
    }

    public async Task<ApiResponse<GetSolutionsDto>> GetByIdAsync(int id)
    {
        var solution = await repository.GetSolution(s => s.Id == id);
        if (solution == null)
        {
            return new ApiResponse<GetSolutionsDto>(HttpStatusCode.NotFound, "Solution Not Found");
        }

        var result = new GetSolutionsDto()
        {
            Id = solution.Id,
            Description = solution.Description,
            CreatedAt = solution.CreatedAt,
            ProfileImagePath = solution.ProfileImagePath,
            IssueId = solution.IssueId,
        };
        return new ApiResponse<GetSolutionsDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddSolutionDto request)
    {
        var solution = new Solution()
        {
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            IssueId = request.IssueId,
        };

        if (request.ProfileImage != null && request.ProfileImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(request.ProfileImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "Invalid file type");

            if (request.ProfileImage.Length > 5 * 1024 * 1024) // 5 MB limit
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "File too large");

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");

            try
            {
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ProfileImage.CopyToAsync(stream);
                }

                solution.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
            }
            catch (IOException ex)
            {
                return new ApiResponse<string>(HttpStatusCode.InternalServerError, $"File upload failed: {ex.Message}");
            }
        }

        var result = await repository.CreateSolution(solution);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateSolutionDto request)
    {
        if (id <= 0) return new ApiResponse<string>(HttpStatusCode.BadRequest, "Invalid ID");
        var solution = await repository.GetSolution(s => s.Id == id);
        if (request.ProfileImage != null && request.ProfileImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(request.ProfileImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "Invalid file type");

            if (request.ProfileImage.Length > 5 * 1024 * 1024) // 5 MB limit
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "File too large");

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(solution.ProfileImagePath))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, solution.ProfileImagePath.TrimStart('/'));
                    if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
                }

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ProfileImage.CopyToAsync(stream);
                }

                solution.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
            }
            catch (IOException ex)
            {
                return new ApiResponse<string>(HttpStatusCode.InternalServerError, $"File upload failed: {ex.Message}");
            }
        }

        var result = await repository.UpdateSolution(solution);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var solution = await repository.GetSolution(s => s.Id == id);
        if (solution == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Solution Not Found");
        }

        if (!string.IsNullOrEmpty(solution.ProfileImagePath))
        {
            var filePath = Path.Combine(_environment.WebRootPath, solution.ProfileImagePath.TrimStart('/'));
            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
            catch (IOException ex)
            {
                return new ApiResponse<string>(HttpStatusCode.InternalServerError,
                    $"File deletion failed: {ex.Message}");
            }
        }

        var result = await repository.DeleteSolution(solution);
        return result == 1
            ? new ApiResponse<string>("Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}