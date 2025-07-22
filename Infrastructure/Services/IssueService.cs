using System.Net;
using Domain.Dto.IssueDto;
using Domain.Dto.SolutionDto;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.EmployeeRepositories;
using Infrastructure.Repositories.IssueRepositories;
using Infrastructure.Response;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

public class IssueService(IIssueRepository repository,IWebHostEnvironment _environment) : IIssueService
{
    public async Task<PaginationResponse<List<GetIssuesDto>>> GetAllIssueAsync(IssueFilter filter)
    {
        
        var issue = await repository.GetAll(filter);
        var totalRecords = issue.Count;
        var data = issue
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
        var result = data.Select(i => new GetIssuesDto()
        {
            Id = i.Id,
            Title = i.Title,
            Description = i.Description,
            CreatedAt = i.CreatedAt,
            ProfileImagePath = Path.GetFileName(i.ProfileImagePath),
            CategoryId = i.CategoryId,
            EmployeeId = i.EmployeeId,
            Solutions = i.Solutions?.Select(s => new GetSolutionsDto()
            {
                Id = s.Id,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                ProfileImagePath = Path.GetFileName(s.ProfileImagePath),
                IssueId = s.IssueId
            }).ToList()
        }).ToList();
        return new PaginationResponse<List<GetIssuesDto>>(result, totalRecords, filter.PageNumber, filter.PageSize);
    }

    public async Task<ApiResponse<GetIssuesDto>> GetByIdAsync(int id)
    {
        var issue = await repository.GetIssue(i => i.Id == id);
        if (issue == null)
        {
            return new ApiResponse<GetIssuesDto>(HttpStatusCode.NotFound, "Issue NotFound ");
        }

        var result = new GetIssuesDto()
        {
            Id = issue.Id,
            Title = issue.Title,
            Description = issue.Description,
            CreatedAt = issue.CreatedAt,
            ProfileImagePath = Path.GetFileName(issue.ProfileImagePath),
            CategoryId = issue.CategoryId,
            EmployeeId = issue.EmployeeId,
            Solutions = issue.Solutions?.Select(s => new GetSolutionsDto()
            {
                Id = s.Id,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                ProfileImagePath = Path.GetFileName(s.ProfileImagePath),
                IssueId = s.IssueId
            }).ToList()
        };
        return new ApiResponse<GetIssuesDto>(result);
    }

    public async Task<ApiResponse<string>> CreateAsync(AddIssueDto request)
    {
        var issue = new Issue()
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            CategoryId = request.CategoryId,
            EmployeeId = request.EmployeeId,
        };
        if (request.ProfileImage != null && request.ProfileImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            //извлекает например photo.png -> .png 
            var fileExtension = Path.GetExtension(request.ProfileImage.FileName).ToLowerInvariant();//-чтобы не зависить от регистра .JPG->.jpg
            
             //проверяет allowedExtensions == fileExtension 
            if (!allowedExtensions.Contains(fileExtension))
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "Invalid file type");
            
            if (request.ProfileImage.Length > 5 * 1024 * 1024) // 5 MB limit
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "File too large");
            //задет уникальное имя чтоб не было конфликта 
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            //это путь к папке куда сохранится 
            // /tmp это временное хранилише в рендере потом нужн изменить на wwwroot WebRootPath
            var uploadsFolder = Path.Combine("/tmp", "uploads", "profiles");

            try
            {
                //Проверяем, существует ли уже папка по пути uploadsFolder если нет то создает папку 
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);  
                //Формируем абсолютный путь к файлу, куда он будет записан.
                //uploadsFolder = "C:\\MyApp\\wwwroot\\uploads\\profiles"
                //uniqueFileName = "a3e1f2c9-fab6-4b8e-a153.png"
                //filePath = "C:\\MyApp\\wwwroot\\uploads\\profiles\\a3e1f2c9-fab6-4b8e-a153.png"
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ProfileImage.CopyToAsync(stream);
                }

                issue.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
            }
            catch (IOException ex)
            {
                return new ApiResponse<string>(HttpStatusCode.InternalServerError, $"File upload failed: {ex.Message}");
            }
        }

        var result = await repository.CreateIssue(issue);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK,"Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, UpdateIssueDto request)
    {
        if (id <= 0) return new ApiResponse<string>(HttpStatusCode.BadRequest, "Invalid ID");
        var issue = await repository.GetIssue(i => i.Id == id);
        if (issue == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Issue Not Found ");
        }

        issue.Title = request.Title;
        issue.Description = request.Description;
        issue.CreatedAt = request.CreatedAt;
        issue.CategoryId = request.CategoryId;
        issue.EmployeeId = request.EmployeeId;
        if (request.ProfileImage != null && request.ProfileImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(request.ProfileImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "Invalid file type");

            if (request.ProfileImage.Length > 5 * 1024 * 1024) // 5 MB 
                return new ApiResponse<string>(HttpStatusCode.BadRequest, "File too large");

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            // /tmp это временное хранилише потом нужн изменить на wwwroot WebRootPath
            var uploadsFolder = Path.Combine("/tmp", "uploads", "profiles"); 
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(issue.ProfileImagePath))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, issue.ProfileImagePath.TrimStart('/'));
                    if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
                }

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ProfileImage.CopyToAsync(stream);
                }

                issue.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
            }
            catch (IOException ex)
            {
                return new ApiResponse<string>(HttpStatusCode.InternalServerError, $"File upload failed: {ex.Message}");
            }
        }

        var result = await repository.UpdateIssue(issue);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK,"Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var issue = await repository.GetIssue(q => q.Id == id);
        if (issue == null)
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, "Issue not found");
        }

        if (!string.IsNullOrEmpty(issue.ProfileImagePath))
        {
            var filePath = Path.Combine(_environment.WebRootPath, issue.ProfileImagePath.TrimStart('/'));
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

        var result = await repository.DeleteIssue(issue);
        return result == 1
            ? new ApiResponse<string>(HttpStatusCode.OK,"Success")
            : new ApiResponse<string>(HttpStatusCode.BadRequest, "Failed");
    }
}