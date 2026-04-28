using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Domain.ValueObjects;

namespace TaskService.Infrastructure.Persistence;

public class FakeDataFactory
{
    public static IEnumerable<(Guid id, User user)> GetUsers()
    {
        yield return (
            id: Guid.Parse("019d58e9-98f4-7638-8fc0-f5e0a6809ec9"),
            user: User.Create(
                keycloakId: Guid.Parse("e24e7bca-2ec4-4ba9-9106-18ba02272c93"),
                userName: new UserName("test"),
                email: new EmailAdress("test@test.ru"),
                fullName: "Test Testov"));
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            user: User.Create(
                keycloakId: Guid.Parse("a0000000-0000-0000-0000-000000000001"),
                userName: new UserName("alice"),
                email: new EmailAdress("alice@example.com"),
                fullName: "Alice Anderson"));

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            user: User.Create(
                keycloakId: Guid.Parse("a0000000-0000-0000-0000-000000000002"),
                userName: new UserName("bob"),
                email: new EmailAdress("bob@example.com"),
                fullName: "Bob Brown"));

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000003"),
            user: User.Create(
                keycloakId: Guid.Parse("a0000000-0000-0000-0000-000000000003"),
                userName: new UserName("carol"),
                email: new EmailAdress("carol@example.com"),
                fullName: "Carol Chen"));

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000004"),
            user: User.Create(
                keycloakId: Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                userName: new UserName("badboy"),
                email: new EmailAdress("badboy@superbadboy.com"),
                fullName: "Сын маминой подруги"));

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000777"),
            user: User.Create(
                keycloakId: Guid.Parse("a0000000-0000-0000-0000-000000000777"),
                userName: new UserName("BEST"),
                email: new EmailAdress("Best@TheBest.com"),
                fullName: "BEST THE BEST"));
    }

    public static IEnumerable<(Guid id, Workspace workspace)> GetWorkspaces()
    {
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            workspace: Workspace.Create(name: "Acme Corp"));

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            workspace: Workspace.Create(name: "Test Lab"));

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000777"),
            workspace: Workspace.Create(name: "BEST"));
    }

    public static IEnumerable<(Guid id, WorkspaceMember member)> GetWorkspaceMembers()
    {
        yield return (
            id: Guid.Empty,
            member: WorkspaceMember.Create(
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
                userId: Guid.Parse("019d58e9-98f4-7638-8fc0-f5e0a6809ec9"),
                role: Roles.Creator)
            );

        yield return (
            id: Guid.Empty,
            member: WorkspaceMember.Create(
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000002"),
                userId: Guid.Parse("019d58e9-98f4-7638-8fc0-f5e0a6809ec9"),
                role: Roles.Creator)
            );

        yield return (
            id: Guid.Empty,
            member: WorkspaceMember.Create(
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000002"),
                userId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
                role: Roles.Member)
            );

        yield return (
            id: Guid.Empty,
            member: WorkspaceMember.Create(
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                userId: Guid.Parse("019d58e9-98f4-7638-8fc0-f5e0a6809ec9"),
                role: Roles.Creator)
            );

        yield return (
            id: Guid.Empty,
            member: WorkspaceMember.Create(
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                userId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                role: Roles.Creator)
            );
    }
    public static IEnumerable<(Guid id, Project project)> GetProjects()
    {
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            project: Project.Create(
                name: "Нежно и небрежно",
                description: "Проект по внедрению альфа бета и гамма туалетной бумаги",
                abbreviation: "NaNT",
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000001")
               )
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            project: Project.Create(
                name: "Дикая ярость",
                description: "Проект по захвату креатив менеджеров муравьиного поселения",
                abbreviation: "DY",
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000001")
               )
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000003"),
            project: Project.Create(
                name: "очень срочный и важный",
                description: "Веб сервис который нужен вчера, но никто не будет им пользоваться",
                abbreviation: "UNG",
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000002")
               )
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000004"),
            project: Project.Create(
                name: "Мегасуперпупер",
                description: "Фейковый проект сына друга деректора для вида, чтобы он получил премию как пять топовых сотрудников",
                abbreviation: "MSP",
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000002")
               )
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000777"),
            project: Project.Create(
                name: "Лучший проект",
                description: "Сделаем разработку снова великой! Спасибо за внимание к данному вопросу!",
                abbreviation: "BEST",
                workspaceId: Guid.Parse("00000000-0000-0000-0000-000000000777")
               )
            );
    }

    public static IEnumerable<(Guid id, ProjectMember member)> GetProjectMembers()
    {
        yield return (
            id: Guid.Empty,
            member: ProjectMember.Create(
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000004"),
                userId: Guid.Parse("00000000-0000-0000-0000-000000000004"),
                role: Roles.Viewer,
                joinedAt: default)
            );
    }

    public static IEnumerable<(Guid id, IssueStatus status)> GetIssueStatuses()
    {
        //1
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            status: IssueStatus.Create(
                name: "Новая",
                numberType: 0,
                position: 0,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000001"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            status: IssueStatus.Create(
                name: "В работе",
                numberType: 1,
                position: 1,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000001"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000003"),
            status: IssueStatus.Create(
                name: "Завершена",
                numberType: 2,
                position: 2,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000001"))
            );

        //2
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000004"),
            status: IssueStatus.Create(
                name: "New",
                numberType: 0,
                position: 0,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000002"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000005"),
            status: IssueStatus.Create(
                name: "In process",
                numberType: 1,
                position: 1,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000002"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000006"),
            status: IssueStatus.Create(
                name: "Success",
                numberType: 2,
                position: 2,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000002"))
            );

        //3
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000007"),
            status: IssueStatus.Create(
                name: "Новая",
                numberType: 0,
                position: 0,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000003"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000008"),
            status: IssueStatus.Create(
                name: "В панике",
                numberType: 1,
                position: 1,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000003"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000009"),
            status: IssueStatus.Create(
                name: "В психушке",
                numberType: 2,
                position: 2,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000003"))
            );

        //4
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000010"),
            status: IssueStatus.Create(
                name: "Новая",
                numberType: 0,
                position: 0,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000004"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000011"),
            status: IssueStatus.Create(
                name: "В работе",
                numberType: 1,
                position: 1,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000004"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000012"),
            status: IssueStatus.Create(
                name: "Выполнено",
                numberType: 1,
                position: 1,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000004"))
            );

        //BEST
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000007771"),
            status: IssueStatus.Create(
                name: "Новое важное",
                numberType: 0,
                position: 0,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000007772"),
            status: IssueStatus.Create(
                name: "Важное в работе",
                numberType: 1,
                position: 1,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"))
            );

        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000007773"),
            status: IssueStatus.Create(
                name: "Выполненное важное",
                numberType: 2,
                position: 2,
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"))
            );
    }

    public static IEnumerable<(Guid id, Issue issue)> GetIssues()
    {
        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Issue.Create(
                name: "Провести тесты предметной области",
                description: "Провести тестовые испытания представленных прототипов конкурентов",
                key: "NaNT-0001",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
                numberIssueType: 0,
                numberIssuePriority: 0,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Issue.Create(
                name: "Планирование",
                description: "Распланировать задачи с сроком выполнения вчера",
                key: "UNG-0001",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000003"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000000007"),
                numberIssueType: 0,
                numberIssuePriority: 0,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000003"),
            Issue.Create(
                name: "Придти на работу",
                description: null,
                key: "UNG-0001",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000004"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000000010"),
                numberIssueType: 0,
                numberIssuePriority: 1,
                dueDate: DateTimeOffset.UtcNow.AddDays(-180))
            );

        //BEST
        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000004"),
            Issue.Create(
                name: "Написать пост",
                description: "Написать пост что завершил 8 проектов",
                key: "BEST-0001",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007771"),
                numberIssueType: 0,
                numberIssuePriority: 4,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000005"),
            Issue.Create(
                name: "Поставить задачу",
                description: "Попросить чтобы кто-то сказал что я завершил 8 проектов",
                key: "BEST-0004",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007771"),
                numberIssueType: 0,
                numberIssuePriority: 4,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000006"),
            Issue.Create(
                name: "Возврат средств",
                description: "Вернуть деньги за повышенную ставку",
                key: "BEST-0006",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007771"),
                numberIssueType: 0,
                numberIssuePriority: 4,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000007"),
            Issue.Create(
                name: "Дать интервью",
                description: "Дать интервью что завершил 8 проектов",
                key: "BEST-0002",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007772"),
                numberIssueType: 0,
                numberIssuePriority: 4,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000008"),
            Issue.Create(
                name: "Заблокировать проход к кофе машине",
                description: "Держать заблокированным проход к кофе машине, пока не согласится на сделку",
                key: "BEST-0007",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007772"),
                numberIssueType: 0,
                numberIssuePriority: 4,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000009"),
            Issue.Create(
                name: "Обвинить соседний отдел",
                description: "Обвинить соседний отдел что они Python разработчики и что им нужно больше вкладываться в кофе",
                key: "BEST-0002",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007772"),
                numberIssueType: 0,
                numberIssuePriority: 4,
                dueDate: null)
            );

        yield return (
            Guid.Parse("00000000-0000-0000-0000-000000000010"),
            Issue.Create(
                name: "Напомнание",
                description: "Напомнить всем что завершил 8 проектов",
                key: "BEST-0003",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
                taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007773"),
                numberIssueType: 0,
                numberIssuePriority: 4,
                dueDate: null)
            );

        yield return (
           Guid.Parse("00000000-0000-0000-0000-000000000011"),
           Issue.Create(
               name: "Повысить ставку",
               description: "Повысить ставку за получение совета",
               key: "BEST-0005",
               projectId: Guid.Parse("00000000-0000-0000-0000-000000000777"),
               taskStatusId: Guid.Parse("00000000-0000-0000-0000-000000007773"),
               numberIssueType: 0,
               numberIssuePriority: 4,
               dueDate: null)
           );
    }

    public static IEnumerable<(Guid id, Tag tag)> GetTags()
    {
        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Tag.Create(
                name: "Пафос",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777")));


        yield return (
            id: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Tag.Create(
                name: "Великий",
                projectId: Guid.Parse("00000000-0000-0000-0000-000000000777")));
    }

    public static IEnumerable<object> Attachments { get; private set; } = null!;

    public async Task Seed(TaskServiceDbContext dbcontext, CancellationToken ct = default)
    {
        await SeedUsers(dbcontext, ct);
        await SeedWorkspace(dbcontext, ct);
        await SeedWorkspaceMembers(dbcontext, ct);
        await SeedProjects(dbcontext, ct);
        await SeedProjectMembers(dbcontext, ct);
        await SeedStatus(dbcontext, ct);
        await SeedIssues(dbcontext, ct);
        await SeedTags(dbcontext, ct);
        await SeedIssueTags(dbcontext, ct);
    }

    public void Seed(TaskServiceDbContext dbcontext)
    {
        SeedUsers(dbcontext).Wait();
        SeedWorkspace(dbcontext).Wait();
        SeedWorkspaceMembers(dbcontext).Wait();
        SeedProjects(dbcontext).Wait();
        SeedProjectMembers(dbcontext).Wait();
        SeedStatus(dbcontext).Wait();
        SeedIssues(dbcontext).Wait();
        SeedTags(dbcontext).Wait();
        SeedIssueTags(dbcontext).Wait();
    }

    public async Task SeedUsers(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetUsers())
        {
            if (!await context.Users.AnyAsync(x => x.Id == typle.id, ct))
            {
                context.Entry(typle.user).Property(p => p.Id).CurrentValue = typle.id;
                context.Users.Add(typle.user);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedWorkspace(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetWorkspaces())
        {
            if (!await context.Workspaces.AnyAsync(x => x.Id == typle.id, ct))
            {
                context.Entry(typle.workspace).Property(p => p.Id).CurrentValue = typle.id;
                context.Workspaces.Add(typle.workspace);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedWorkspaceMembers(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetWorkspaceMembers())
        {
            WorkspaceMember member = typle.member;
            if (!await context.WorkspaceMembers.AnyAsync(x => x.WorkspaceId == member.WorkspaceId && x.UserId == member.UserId, ct))
            {
                context.WorkspaceMembers.Add(member);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedProjects(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetProjects())
        {
            if (!await context.Projects.AnyAsync(x => x.Id == typle.id, ct))
            {
                context.Entry(typle.project).Property(p => p.Id).CurrentValue = typle.id;
                context.Projects.Add(typle.project);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedProjectMembers(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetProjectMembers())
        {
            ProjectMember member = typle.member;
            if (!await context.ProjectMembers.AnyAsync(x => x.ProjectId == member.ProjectId && x.UserId == member.UserId, ct))
            {
                context.ProjectMembers.Add(member);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedStatus(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetIssueStatuses())
        {
            if (!await context.IssueStatus.AnyAsync(x => x.Id == typle.id, ct))
            {
                context.Entry(typle.status).Property(p => p.Id).CurrentValue = typle.id;
                context.IssueStatus.Add(typle.status);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedIssues(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetIssues())
        {
            if (!await context.Issues.AnyAsync(x => x.Id == typle.id, ct))
            {
                context.Entry(typle.issue).Property(p => p.Id).CurrentValue = typle.id;
                context.Issues.Add(typle.issue);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedTags(TaskServiceDbContext context, CancellationToken ct = default)
    {
        foreach (var typle in FakeDataFactory.GetTags())
        {
            if (!await context.Tags.AnyAsync(x => x.Id == typle.id, ct))
            {
                context.Entry(typle.tag).Property(p => p.Id).CurrentValue = typle.id;
                context.Tags.Add(typle.tag);
            }
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task SeedIssueTags(TaskServiceDbContext context, CancellationToken ct = default)
    {

        Tag tag1 = await context.Tags.FirstAsync(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001"), ct);
        Tag? tag2 = await context.Tags.FirstAsync(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000002"), ct);

        Issue issue1 = await context.Issues
            .Include(x => x.Tags)
            .FirstAsync(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000007"), ct);

        Issue issue2 = await context.Issues
            .Include(x => x.Tags)
            .FirstAsync(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000008"), ct);

        Issue issue3 = await context.Issues
            .Include(x => x.Tags)
            .FirstAsync(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004"), ct);

        if (!issue1.Tags.Contains(tag1))
            issue1.Tags.Add(tag1);
        if (!issue2.Tags.Contains(tag2))
            issue2.Tags.Add(tag2);

        if (!issue3.Tags.Contains(tag1))
            issue3.Tags.Add(tag1);
        if (!issue3.Tags.Contains(tag2))
            issue3.Tags.Add(tag2);

        context.SaveChanges();
    }
}
