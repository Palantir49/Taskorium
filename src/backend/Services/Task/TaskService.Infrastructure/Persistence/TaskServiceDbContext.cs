using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Persistence;

public class TaskServiceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Issue> Issues { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Workspace> Workspaces { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
