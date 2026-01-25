using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Workspaces;
using TaskService.Application.Commands.Workspaces.Get;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.Workspace.Request;
using TaskService.Contracts.Workspace.Response;

namespace TaskService.Api.Controllers;

/// <summary>
///     Контроллер для работы с рабочими пространствами
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class WorkSpacesController(IDispatcher dispatcher) : Controller
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
        var query = new GetWorkspaceByIdQuery(id);
        var workspaceResponse = await dispatcher.SendAsync(query);
        if (workspaceResponse == null)
        {
            return NotFound();
        }

        return Ok(workspaceResponse);
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
        var workspaceResponse = await dispatcher.SendAsync(createWorkspaceRequest.ToCommand());
        return CreatedAtAction(nameof(GetWorkspaceByIdAsync), new { id = workspaceResponse.id }, workspaceResponse);
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
