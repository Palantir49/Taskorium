using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Projects;
using TaskService.Application.Commands.Projects.Command;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Features.Projects.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.Project.Requests;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Api.Controllers;

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
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> CreateProjectAsync([FromBody] CreateProjectRequest createProjectRequest)
    {
        ProjectCreateCommand createProjectCommand = createProjectRequest.ToCommand();
        ProjectResponse response = await dispatcher.SendAsync(createProjectCommand);
        return CreatedAtAction(nameof(GetProjectByIdAsync), new { id = response.Id }, response);
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
    [HttpGet("{id:guid}")]
    [ActionName("GetProjectByIdAsync")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> GetProjectByIdAsync(Guid id)
    {
        ProjectGetByIdQuery query = new ProjectGetByIdQuery(id);
        ProjectResponse response = await dispatcher.SendAsync(query);
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
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> UpdateProjectAsync(Guid id,
        [FromBody] UpdateProjectRequest updateProjectRequest)
    {
        ProjectUpdateCommand command = ProjectMapping.ProjectUpdateCommand(id, updateProjectRequest);
        ProjectResponse response = await dispatcher.SendAsync(command);
        return Ok(response);
    }

    /// <summary>
    ///     Получить все задачи проекта
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     PUT /api/v1/Projects/guid/tasks
    ///     {
    ///     }
    /// </remarks>
    /// <param name="id">Идентификатор проекта</param>
    /// <returns></returns>
    /// <response code="200">Данные о задачах проекта успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найден проект для обновления</response>
    [HttpPut("{id:guid}/tasks")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IEnumerable<IssueResponse>>> GetProjectByProjectidAsync(Guid id)
    {//FAQ: а это нормальный возвращаемый тип?
        IssueGetByProjectIdQuery query = new IssueGetByProjectIdQuery(id);
        IEnumerable<IssueResponse> response = await dispatcher.SendAsync(query);
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
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProjectAsync(Guid id)
    {
        ProjectDeleteByIdCommand command = new(id);
        int response = await dispatcher.SendAsync(command);
        return NoContent();
    }
}
