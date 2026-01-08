using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Persistence;

public class TaskServiceDbContext : DbContext
{
    public TaskServiceDbContext(DbContextOptions options) : base(options) { }
    public TaskServiceDbContext() { }
    internal DbSet<Issue> Issues { get; set; } = null!;
    internal DbSet<Project> Projects { get; set; } = null!;
    internal DbSet<Workspace> Workspaces { get; set; } = null!;
    //internal DbSet<User> Users { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
