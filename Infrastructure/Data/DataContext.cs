using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Department> Departments {get;set;}
    public DbSet<SubDepartment> SubDepartments {get;set;}
    public DbSet<Category> Categories {get;set;}
    public DbSet<Issue> Issues {get;set;}
    public DbSet<Solution> Solutions {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //Department → SubDepartment (1 ко многим)
        modelBuilder.Entity<Department>()
            .HasMany(d => d.SubDepartments)
            .WithOne(sd => sd.Department)
            .HasForeignKey(sd => sd.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade); 

        // SubDepartment → Categories (1 ко многим)
        modelBuilder.Entity<SubDepartment>()
            .HasMany(sd => sd.Categories)
            .WithOne(c => c.SubDepartment)
            .HasForeignKey(c => c.SubDepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Category → Issues (1 ко многим)
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Issues)
            .WithOne(i => i.Category)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //Issue → Solution (1 ко многим)
        modelBuilder.Entity<Issue>()
            .HasMany(s => s.Solutions)
            .WithOne(i => i.Issue)
            .HasForeignKey(i => i.IssueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}