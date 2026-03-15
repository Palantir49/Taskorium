using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Features.WorkspaceMembers.AddUser;
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
    ///     Создать новый проект
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     POST /api/v1/Projects
    ///     {
    ///     }
    /// </remarks>
    /// <param name="createProjectRequest">Данные о новом проекте</param>
    /// <returns></returns>
    /// <response code="201">Новый проект успешно создан</response>
    /// <response code="400">Некорректный запрос</response>
    [Authorize(Policy = "CanCreateProject")]
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> CreateProjectAsync(
        [FromBody] CreateProjectRequest createProjectRequest)
    {
        var createProjectCommand = createProjectRequest.ToCommand();
        var response = await dispatcher.SendAsync(createProjectCommand);
        return CreatedAtAction(nameof(GetProjectByIdAsync), new { id = response.Id }, response);
    }

    /// <summary>
    ///     Добавить пользователя в проект
    /// </summary>
    /// <param name="command">Данные о проекте, пользователе и его роли</param>
    /// <returns></returns>
    /// <response code="201">Пользователь успешно добавлен в проект</response>
    /// <response code="400">Некорректный запрос</response>
    [Authorize(Policy = "CanAddUserToProject")]
    [HttpPost("AddProjectMember")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> AddUserToProjectAsync([FromBody] AddProjectMemberCommand command)
    {
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
    /// <param name="id">Идентификатор проекта</param>
    /// <response code="200">Данные о проекте успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект по заданному id</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{id:guid}")]
    [ActionName("GetProjectByIdAsync")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> GetProjectByIdAsync(Guid id)
    {
        var query = new ProjectGetByIdQuery(id);
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
    /// <param name="id">Идентификатор проекта</param>
    /// <param name="updateProjectRequest">Обновленные данные проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о проекте успешно обновлены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект для обновления</response>
    [Authorize(Policy = "CanUpdateProject")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> UpdateProjectAsync(Guid id,
        [FromBody] UpdateProjectRequest updateProjectRequest)
    {
        var command = ProjectMapping.ProjectUpdateCommand(id, updateProjectRequest);
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
    /// <param name="id">Идентификатор проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о задачах проекта успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{id:guid}/Issues")]
    [ProducesResponseType(typeof(IEnumerable<IssueResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<IssueResponse>>> GetIssuesByProjectidAsync(Guid id)
    {
        //FAQ: а это нормальный возвращаемый тип?
        var query = new IssueGetByProjectIdQuery(id);
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
    /// <param name="id">Идентификатор проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о статусах задачи проекта успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpGet("{id:guid}/IssueStatuses")]
    [ProducesResponseType(typeof(IEnumerable<IssueStatusResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<IssueResponse>>> GetIssuesStatusByProjectidAsync(Guid id)
    {
        //FAQ: а это нормальный возвращаемый тип?
        var query = new IssueStatusGetByProjectIdQuery(id);
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
    /// <param name="id">Идентификатор проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о типах задач проекта успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект</response>
    [HttpGet("{id:guid}/Tags")]
    [ProducesResponseType(typeof(IEnumerable<TagResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<TagResponse>>> GetTagsByProjectidAsync(Guid id)
    {
        //FAQ: а это нормальный возвращаемый тип?
        var query = new TagGetByProjectIdQuery(id);
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
    /// <param name="id">Идентификатор задачи для удаления</param>
    /// <returns></returns>
    /// <response code="204">Задача успешно удалена</response>
    /// <response code="404">Не найдена задача для удаления</response>
    [Authorize(Policy = "CanViewProject")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProjectAsync(Guid id)
    {
        ProjectDeleteByIdCommand command = new(id);
        var response = await dispatcher.SendAsync(command);
        return NoContent();
    }
}
