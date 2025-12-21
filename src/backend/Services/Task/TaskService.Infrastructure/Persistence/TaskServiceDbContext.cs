using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;


namespace TaskService.Infrastructure.Persistence;

public class TaskServiceDbContext : DbContext
{
    public DbSet<Issue> Issues { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;

    public TaskServiceDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
