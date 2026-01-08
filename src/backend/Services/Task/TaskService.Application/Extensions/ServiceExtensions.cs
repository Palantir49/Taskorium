using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Abstractions;
using TaskService.Application.Commands.Issues.Handler;
using TaskService.Application.Commands.Projects.Handler;
using TaskService.Application.Commands.Users.Create;
using TaskService.Application.Commands.Users.Get;
using TaskService.Application.Commands.Workspaces.Create;
using TaskService.Application.Commands.Workspaces.Get;
using TaskService.Application.Mediator;

namespace TaskService.Application.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureApplicationLayer()
        {
            services.AddScoped<CreateWorkspaceHandler>();
            services.AddScoped<CreateIssueHandler, CreateIssueHandler>();
            services.AddScoped<CreateProjectHandler, CreateProjectHandler>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandHandler<CreateWorkspaceCommand, CreateWorkspaceResult>, CreateWorkspaceHandler>();
            services.AddScoped<IQueryHandler<GetWorkspaceByIdQuery, GetWorkspacebyIdResult>, GetWorkspaceHandler>();
            //services.AddScoped<ICommandHandler<CreateUserCommand, CreateUserResult>, CreateUserHandler>();
        }
    }
}
