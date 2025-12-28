using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Interfaces;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.Workspace.Request;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Api.Controllers;

/// <summary>
///     Контроллер для работы с рабочими пространствами
/// </summary>
/// <param name="workspaceService">Сервис для работы с задачами</param>
[ApiController]
[Route("api/v1/[controller]")]
public class WorkSpacesController(IWorkspaceService workspaceService) : Controller
{
    /// <summary>
    ///     Получить рабочей области по Id
    /// </summary>
    /// ///
    /// <remarks>
    ///     Пример запроса:
    ///     GET /api/v1/Workspaces/guid
    /// </remarks>
    /// <param name="id">Идентификатор рабочей области</param>
    /// <response code="200">Данные о рабочей области успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найдена рабочая область по заданному id</response>
    [HttpGet("{id:guid}")]
    [ActionName("GetWorkspaceByIdAsync")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> GetWorkspaceByIdAsync(Guid id)
    {
        var taskResponse = await workspaceService.GetWorkspaceByIdAsync(id);
        if (taskResponse == null)
        {
            return NotFound();
        }

        return Ok(taskResponse);
    }

    /// <summary>
    ///     Создать новою рабочую область
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     POST /api/v1/Workspaces
    ///     {
    ///     }
    /// </remarks>
    /// <param name="createWorkspaceRequest">Данные о новой задаче</param>
    /// <returns></returns>
    /// <response code="201">Новая задача успешно создана</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPost]
    [ProducesResponseType(typeof(WorkspaceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<WorkspaceResponse>> CreateWorkspaceAsync(
        [FromBody] CreateWorkspaceRequest createWorkspaceRequest)
    {
        var workspaceResponse = await workspaceService.CreateWorkspaceAsync(createWorkspaceRequest);
        return CreatedAtAction(nameof(GetWorkspaceByIdAsync), new { id = workspaceResponse.Id }, workspaceResponse);
    }

    /// <summary>
    ///     Удалить задачу по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     DELETE /api/v1/Issues/1
    /// </remarks>
    /// <param name="id">Идентификатор задачи для удаления</param>
    /// <returns></returns>
    /// <response code="204">Задача успешно удалена</response>
    /// <response code="404">Не найдена задача для удаления</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> DeleteWorkspaceAsync(Guid id)
    {
        return Task.FromResult<IActionResult>(NoContent());
    }
}
