using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Workspaces;
using TaskService.Application.Commands.Workspaces.Create;
using TaskService.Application.Commands.Workspaces.Get;
using TaskService.Application.Features.Workspaces.Delete;
using TaskService.Application.Features.Workspaces.Update;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.Workspace.Request;
using TaskService.Contracts.Workspace.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
    [ProducesResponseType(typeof(GetWorkspacebyIdResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GetWorkspacebyIdResult>> GetWorkspaceByIdAsync(Guid id)
    {
        var query = new GetWorkspaceByIdQuery(id);
        var response = await dispatcher.SendAsync(query);
        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
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
    /// <param name="command">Данные о новой задаче</param>
    /// <returns></returns>
    /// <response code="201">Новая задача успешно создана</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPost]
    [ProducesResponseType(typeof(WorkspaceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<WorkspaceResponse>> CreateWorkspaceAsync(
        [FromBody] CreateWorkspaceCommand command)
    {
        var response = await dispatcher.SendAsync(command);
        return CreatedAtAction(nameof(GetWorkspaceByIdAsync), new { id = response.id }, response);
    }

    /// <summary>
    ///     Обновление названия рабочей области
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     POST /api/v1/Workspaces
    ///     {
    ///     }
    /// </remarks>
    /// <param name="command">Id рабочей области и имя рабочей области</param>
    /// <returns></returns>
    /// <response code="201">Имя рабочей области успешно обновлено</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPatch]
    [ProducesResponseType(typeof(WorkspaceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<WorkspaceResponse>> UpdateWorkspaceAsync(
        [FromBody] UpdateWorkspaceNameCommand command)
    {
        var response = await dispatcher.SendAsync(command);
        return CreatedAtAction(nameof(GetWorkspaceByIdAsync), new { id = response.id }, response);
    }
    /// <summary>
    ///     Удалить рабочую область
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     DELETE /api/v1/Issues/1
    /// </remarks>
    /// <param name="id">Идентификатор задачи для удаления</param>
    /// <returns></returns>
    /// <response code="204">Рабочая область успешно удалена</response>
    /// <response code="404">Не найдена рабочая область для удаления пользователя</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteWorkspaceByIdResult>> DeleteWorkspaceByIdAsync(Guid id)
    {
        var response = await dispatcher.SendAsync(new DeleteWorkspaceByIdCommand(id));
        return Ok(response);
    }
}
