using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Projects.Command;
using TaskService.Application.Commands.Workspaces.Create;
using TaskService.Application.Commands.Workspaces.Get;
using TaskService.Application.Features.Users.Get;
using TaskService.Application.Features.WorkspaceMembers.AddUser;
using TaskService.Application.Features.Workspaces.Delete;
using TaskService.Application.Features.Workspaces.Get;
using TaskService.Application.Features.Workspaces.Update;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Requests;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.Project.Requests;
using TaskService.Contracts.Project.Responses;
using TaskService.Contracts.Workspace.Request;
using TaskService.Contracts.Workspace.Response;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mapping;
namespace TaskService.Api.Controllers;

/*TODO Action: delete user from workspace
  TODO Action: SetAdmin in workspace?
  TODO Action: UnSetAdmin in workspace?
  TODO Get all users from workspace
 */

/// <summary>
///     Контроллер для работы с рабочими пространствами
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class WorkspaceController(IDispatcher dispatcher) : Controller
{
    /// <summary>
    ///     Получить рабочей области по Id
    /// </summary>
    /// ///
    /// <remarks>
    ///     Пример запроса:
    ///     GET /api/v1/Workspaces/guid
    /// </remarks>
    /// <param name="workspaceId">Идентификатор рабочей области</param>
    /// <response code="200">Данные о рабочей области успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найдена рабочая область по заданному id</response>
    [Authorize(Policy = "CanViewWorkSpace")]
    [HttpGet("{workspaceId:guid}")]
    [ActionName("GetWorkspaceByIdAsync")]
    [ProducesResponseType(typeof(GetWorkspacebyIdResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GetWorkspacebyIdResult>> GetWorkspaceByIdAsync([FromRoute] Guid workspaceId)
    {
        var query = new GetWorkspaceByIdQuery(workspaceId);
        var response = await dispatcher.SendAsync(query);
        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    /// <summary>
    ///     Получить список рабочих области
    /// </summary>
    /// ///
    /// <param name="query">Объект пагинации</param>
    /// <response code="200">Список рабочих областей успешно получен</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найдена рабочая область по заданному id</response>
    [HttpGet("workspaces")]
    [ActionName("GetWorkspacePageAsync")]
    [ProducesResponseType(typeof(GetWorkspacebyIdResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GetWorkspacePageResult>> GetWorkspacePageAsync(
        [FromQuery] GetWorkspacePageQuery query)
    {
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
    [Authorize(Policy = "CanCreateWorkSpace")]
    [HttpPost]
    [ProducesResponseType(typeof(WorkspaceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<WorkspaceResponse>> CreateWorkspaceAsync(
        [FromBody] CreateWorkspaceCommand command)
    {
        var response = await dispatcher.SendAsync(command);
        return CreatedAtAction(nameof(GetWorkspaceByIdAsync), new { response.Id }, response);
    }

    /// <summary>
    ///     Добавить пользователя в рабочую область
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     POST /api/v1/Workspaces/id/user
    ///     {
    ///     }
    /// </remarks>
    /// <param name="workspaceId">Id рабочей области</param>
    /// <param name="request">Данные добавляемого пользователя</param>
    /// <returns></returns>
    /// <response code="201">Новая задача успешно создана</response>
    /// <response code="400">Некорректный запрос</response>
    [Authorize(Policy = "CanAddUserToWorkSpace")]
    [HttpPost("{workspaceId:guid}/users/")]
    [ProducesResponseType(typeof(AddWorkspaceMemberResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<WorkspaceResponse>> AddUserToWorkspaceAsync([FromRoute] Guid workspaceId, [FromBody] AddUserToWorkspaceRequest request)
    {
        var command = new AddWorkspaceMemberCommand(WorkspaceId: workspaceId, UserId: request.UserId, Role: request.Role);
        var response = await dispatcher.SendAsync(command);
        return Ok(response);
    }
    /// <summary>
    ///     Создать новою задачу
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     POST /api/v1/Issues
    ///     {
    ///     }
    /// </remarks>
    /// <param name="workspaceId"></param>
    /// <param name="projectId"></param>
    /// <param name="createIssueRequest">Данные о новой задаче</param>
    /// <returns></returns>
    /// <response code="201">Новая задача успешно создана</response>
    /// <response code="400">Некорректный запрос</response>
    [Authorize(Policy = "CanCreateTask")]
    [HttpPost("{workspaceId:guid}/project/{projectId:guid}/issue")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> CreateIssueAsync([FromRoute] Guid workspaceId, [FromRoute] Guid projectId, [FromForm] IssueCreateRequest createIssueRequest)
    {
        var createIssueCommand = createIssueRequest.ToCommand();
        var response = await dispatcher.SendAsync(createIssueCommand);
        return Ok(response);
    }
    /// <summary>
    ///     Создать проект в рабочей области рабочую область
    /// </summary>
    /// <param name="workspaceId">Id рабочей области</param>
    /// <param name="request">Данные создаваемого проекта</param>
    /// <returns></returns>
    /// <response code="201">Проект в рабочей области успешно создана</response>
    /// <response code="400">Некорректный запрос</response>
    [Authorize(Policy = "CanCreateProject")]
    [HttpPost("{workspaceId:guid}/project/")]
    [ProducesResponseType(typeof(AddWorkspaceMemberResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProjectResponse>> CreateProjectAsync([FromRoute] Guid workspaceId, [FromBody] CreateProjectRequest request)
    {
        var command = new ProjectCreateCommand(Name: request.Name,
                                               Description: request.Description,
                                               Abbreviation: request.Abbreviation,
                                               WorkspaceId: workspaceId,
                                               UserId: request.UserId);
        var response = await dispatcher.SendAsync(command);
        return Ok(response);
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
    /// <param name="workspaceId">Id рабочей области и имя рабочей области</param>
    /// <param name="command">Id рабочей области и имя рабочей области</param>
    /// <returns></returns>
    /// <response code="201">Имя рабочей области успешно обновлено</response>
    /// <response code="400">Некорректный запрос</response>
    [Authorize(Policy = "CanUpdateWorkSpace")]
    [HttpPatch("{workspaceId:guid}")]
    [ProducesResponseType(typeof(WorkspaceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<WorkspaceResponse>> UpdateWorkspaceAsync([FromRoute] Guid workspaceId,
        [FromBody] UpdateWorkspaceNameCommand command)
    {
        var response = await dispatcher.SendAsync(command);
        return CreatedAtAction(nameof(GetWorkspaceByIdAsync), new { response.id }, response);
    }

    /// <summary>
    ///     Удалить рабочую область
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     DELETE /api/v1/Issues/1
    /// </remarks>
    /// <param name="workspaceId">Идентификатор задачи для удаления</param>
    /// <returns></returns>
    /// <response code="204">Рабочая область успешно удалена</response>
    /// <response code="404">Не найдена рабочая область для удаления пользователя</response>
    [Authorize(Policy = "CanDeleteWorkSpace")]
    [HttpDelete("{workspaceId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteWorkspaceByIdResult>> DeleteWorkspaceByIdAsync([FromRoute] Guid workspaceId)
    {
        var response = await dispatcher.SendAsync(new DeleteWorkspaceByIdCommand(workspaceId));
        return Ok(response);
    }
}
