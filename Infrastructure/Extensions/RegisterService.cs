using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.DepartmentRepositories;
using Infrastructure.Repositories.IssueRepositories;
using Infrastructure.Repositories.SolutionRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class RegisterService
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));
        
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        
        services.AddScoped<IIssueRepository, IssueRepository>();
        services.AddScoped<IIssueService, IssueService>();
        
        services.AddScoped<ISolutionRepository, SolutionRepository>();
        services.AddScoped<ISolutionService, SolutionService>();
    }
}