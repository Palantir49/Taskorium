using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Features.Projects.Read.GetProjectById;
using TaskService.Application.Features.Projects.Read.GetProjectByWorkspaceId;
using TaskService.Application.Features.Projects.Read.GetProjectMembers;
using TaskService.Application.Features.Projects.Write.AddProjectMember;
using TaskService.Application.Features.Projects.Write.DeleteProjectById;
using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.IssueStatus;
using TaskService.Contracts.Project.Requests;
using TaskService.Contracts.Project.Responses;
using TaskService.Contracts.Tag;

namespace TaskService.Api.Controllers;

/*TODO Action: delete user from project
  TODO Action: SetAdmin in project?
  TODO Action: UnSetAdmin in project?
  TODO Get all users from project
 */

/// <summary>
///     Контроллер проектов
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController(IDispatcher dispatcher) : Controller
{

    /// <summary>
    ///     Добавить пользователя в проект
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="request">Данные о проекте, пользователе и его роли</param>
    /// <returns></returns>
    /// <response code="201">Пользователь успешно добавлен в проект</response>
    /// <response code="400">Некорректный запрос</response>
    [Authorize(Policy = "CanAddUserToProject")]
    [HttpPost("{projectId:guid}/projectMember")]
    [ActionName("AddUserToProjectAsync")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> AddUserToProjectAsync([FromRoute] Guid projectId, [FromBody] AddProjectMemberRequest request)
    {
        var command = new AddProjectMemberCommand(ProjectId: projectId,
                                                  UserId: request.UserId,
                                                  RoleDto: request.RoleDto);
        var response = await dispatcher.SendAsync(command);
        return Ok(response);
    }

    /// <summary>
    ///     Получить данные проекта по Id
    /// </summary>
    /// ///
    /// <remarks>
    ///     Пример запроса:
    ///     GET /api/v1/Projects/guid
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <response code="200">Данные о проекте успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект по заданному id</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{projectId:guid}")]
    [ActionName("GetProjectByIdAsync")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> GetProjectByIdAsync([FromRoute] Guid projectId)
    {
        var query = new GetProjectByIdQuery(projectId);
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
    }

    /// <summary>
    ///     Получить все проекты рабочей области
    /// </summary>
    [Authorize(Policy = "CanViewWorkSpace")]
    [HttpGet("workspace/{workspaceId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetProjectsByWorkspaceIdAsync([FromRoute] Guid workspaceId)
    {
        var query = new GetProjectByWorkspaceIdQuery(workspaceId);
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
    }
    /// <summary>
    ///     Получение пользователей проекта
    /// </summary>
    /// 
    /// <param name="projectId">Идентификатор проекта</param>
    /// <response code="200">Данные о проекте успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект по заданному id</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{projectId:guid}/members")]
    [ActionName("GetProjectMembersAsync")]
    [ProducesResponseType(typeof(ProjectMembersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectMembersResponse>> GetProjectMembersAsync([FromRoute] Guid projectId)
    {
        var query = new GetProjectMembersQuery(projectId);
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
    }
    /// <summary>
    ///     Обновить данные проекта по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     PUT /api/v1/Projects/guid
    ///     {
    ///     }
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="updateProjectRequest">Обновленные данные проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о проекте успешно обновлены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект для обновления</response>
    [Authorize(Policy = "CanUpdateProject")]
    [HttpPut("{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> UpdateProjectAsync([FromRoute] Guid projectId,
        [FromBody] UpdateProjectRequest updateProjectRequest)
    {
        var command = ProjectMapping.ProjectUpdateCommand(projectId, updateProjectRequest);
        var response = await dispatcher.SendAsync(command);
        return Ok(response);
    }

    /// <summary>
    ///     Получить все задачи проекта
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     PUT /api/v1/Projects/guid/Issues
    ///     {
    ///     }
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о задачах проекта успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{projectId:guid}/Issues")]
    [ProducesResponseType(typeof(IEnumerable<IssueResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<IssueResponse>>> GetIssuesByProjectidAsync([FromRoute] Guid projectId)
    {
        //FAQ: а это нормальный возвращаемый тип?
        var query = new IssueGetByProjectIdQuery(projectId);
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
    }

    /// <summary>
    ///     Получить все статусы задачи проекта
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     PUT /api/v1/Projects/guid/IssueStatuses
    ///     {
    ///     }
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о статусах задачи проекта успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{projectId:guid}/IssueStatuses")]
    [ProducesResponseType(typeof(IEnumerable<IssueStatusResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<IssueStatusResponse>>> GetIssuesStatusByProjectidAsync([FromRoute] Guid projectId)
    {
        //FAQ: а это нормальный возвращаемый тип?
        var query = new IssueStatusGetByProjectIdQuery(projectId);
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
    }

    /// <summary>
    ///     Получить все типы задачи проекта
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     PUT /api/v1/Projects/guid/Tags
    ///     {
    ///     }
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о типах задач проекта успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{projectId:guid}/Tags")]
    [ProducesResponseType(typeof(IEnumerable<TagResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<TagResponse>>> GetTagsByProjectidAsync([FromRoute] Guid projectId)
    {
        //FAQ: а это нормальный возвращаемый тип?
        var query = new TagGetByProjectIdQuery(projectId);
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
    }

    /// <summary>
    ///     Удалить проект по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     DELETE /api/v1/Projects/guid
    /// </remarks>
    /// <param name="projectId">Идентификатор задачи для удаления</param>
    /// <returns></returns>
    /// <response code="204">Задача успешно удалена</response>
    /// <response code="404">Не найдена задача для удаления</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProjectAsync([FromRoute] Guid projectId)
    {
        DeleteProjectByIdCommand command = new(projectId);
        var response = await dispatcher.SendAsync(command);
        return NoContent();
    }
}
