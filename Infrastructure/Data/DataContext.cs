using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Department> Departments {get;set;}
    public DbSet<Issue> Issues {get;set;}
    public DbSet<Solution> Solutions {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Issues)
            .WithOne(i => i.Department)
            .HasForeignKey(i => i.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Issue>()
            .HasMany(i => i.Solutions)
            .WithOne(s => s.Issue)
            .HasForeignKey(s => s.IssueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}