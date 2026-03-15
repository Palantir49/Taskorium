using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;

namespace TaskService.Infrastructure.Persistence;

public class TaskServiceDbContext : DbContext
{
    public TaskServiceDbContext(DbContextOptions options) : base(options) { }
    public TaskServiceDbContext() { }
    public DbSet<Issue> Issues { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Workspace> Workspaces { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
    public DbSet<IssueStatus> IssueStatus { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
