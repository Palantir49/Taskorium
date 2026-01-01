using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Handlers.Issues.Command;
using TaskService.Application.Handlers.Issues.Handler;
using TaskService.Application.Handlers.Projects;
using TaskService.Application.Handlers.Projects.Command;
using TaskService.Application.Handlers.Projects.Handler;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.Project.Requests;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Api.Controllers;

/// <summary>
///     Контроллер проектов
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController(CreateProjectHandler createProjectHandler) : Controller
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
    public async Task<ActionResult<ProjectResponse>> CreateIssueAsync([FromBody] CreateProjectRequest createProjectRequest)
    {
        CreateProjectCommand createProjectCommand = createProjectRequest.ToCommand();
        ProjectResponse response = await createProjectHandler.HandleAsync(createProjectCommand);
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
    public Task<ActionResult<ProjectResponse>> GetProjectByIdAsync(Guid id)
    {
        //var taskResponse = await issueService.GetTaskByIdAsync(id);
        //if (taskResponse == null)
        //{
        //    return NotFound();
        //}

        //return Ok(taskResponse);
        return Task.FromResult<ActionResult<ProjectResponse>>(Ok());
    }
}
