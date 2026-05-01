using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Taskorium.ServiceDefaults;
using TaskService.Api.Authorization.Actions;
using TaskService.Api.Authorization.Handlers;
using TaskService.Api.Authorization.Requirements;

namespace TaskService.Api.Extensions;

/// <summary>
///     Extensions for api
/// </summary>
public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        internal void ConfigureJwtAuthentication(IConfiguration configuration)
        {
            var authority = configuration["Authentication:Jwt:Authority"];
            var audience = configuration["Authentication:Jwt:Audience"];

            if ((string.IsNullOrWhiteSpace(authority) || string.IsNullOrWhiteSpace(audience)) &&
                BuildTimeDetector.IsBuildTime)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
                return;
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });
        }


        internal void ConfigureAuthorization()
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("CanViewWorkSpace",
                    p => p.AddRequirements(new WorkSpaceAccessRequirement(WorkSpaceAction.View)))
                .AddPolicy("CanCreateWorkSpace",
                    p => p.AddRequirements(new WorkSpaceAccessRequirement(WorkSpaceAction.Create)))
                .AddPolicy("CanUpdateWorkSpace",
                    p => p.AddRequirements(new WorkSpaceAccessRequirement(WorkSpaceAction.Update)))
                .AddPolicy("CanDeleteWorkSpace",
                    p => p.AddRequirements(new WorkSpaceAccessRequirement(WorkSpaceAction.Delete)))
                .AddPolicy("CanAddUserToWorkSpace",
                    p => p.AddRequirements(new WorkSpaceAccessRequirement(WorkSpaceAction.AddUser)))
                .AddPolicy("CanDeleteUserFromWorkSpace",
                    p => p.AddRequirements(new WorkSpaceAccessRequirement(WorkSpaceAction.DeleteUser)))
                .AddPolicy("CanViewProject", p => p.AddRequirements(new ProjectAccessRequirement(ProjectAction.View)))
                .AddPolicy("CanCreateProject",
                    p => p.AddRequirements(new ProjectAccessRequirement(ProjectAction.Create)))
                .AddPolicy("CanUpdateProject",
                    p => p.AddRequirements(new ProjectAccessRequirement(ProjectAction.Update)))
                .AddPolicy("CanDeleteProject",
                    p => p.AddRequirements(new ProjectAccessRequirement(ProjectAction.Delete)))
                .AddPolicy("CanAddUserToProject",
                    p => p.AddRequirements(new ProjectAccessRequirement(ProjectAction.AddUser)))
                .AddPolicy("CanDeleteUserFromProject",
                    p => p.AddRequirements(new ProjectAccessRequirement(ProjectAction.DeleteUser)))
                .AddPolicy("CanViewTask", p => p.AddRequirements(new IssueAccessRequirement(IssueAction.View)))
                .AddPolicy("CanCreateTask", p => p.AddRequirements(new IssueAccessRequirement(IssueAction.Create)))
                .AddPolicy("CanUpdateTask", p => p.AddRequirements(new IssueAccessRequirement(IssueAction.Update)))
                .AddPolicy("CanDeleteTask", p => p.AddRequirements(new IssueAccessRequirement(IssueAction.Delete)));


            services.AddScoped<IAuthorizationHandler, IssueAccessHandler>();
            services.AddScoped<IAuthorizationHandler, ProjectAccessHandler>();
            services.AddScoped<IAuthorizationHandler, WorkSpaceAccessHandler>();
        }
    }
}
