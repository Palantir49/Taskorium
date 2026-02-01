using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Issues.Command;
using TaskService.Application.Commands.Issues.Query;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Requests;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Api.Controllers;

/// <summary>
///     Контроллер для работы с задачами
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class IssuesController(IDispatcher dispatcher) : Controller
{
    /// <summary>
    ///     Получить все задачи
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     GET /api/v1/Issues
    /// </remarks>
    /// <response code="200">Список задач успешно получен</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpGet]
    [ProducesResponseType(typeof(IssuesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IssuesResponse>> GetAllIssuesAsync()
    {
        var query = new GetAllIssuesQuery();
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
    }

    /// <summary>
    ///     Получить данные задачи по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     GET /api/v1/Issues/guid
    /// </remarks>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="200">Данные о задаче успешно получены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найдена задача по заданному id</response>
    [HttpGet("{id:guid}")]
    [ActionName("GetTaskByIdAsync")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> GetTaskByIdAsync(Guid id)
    {
        IssueGetByIdQuery query = new IssueGetByIdQuery(id);
        IssueResponse response = await dispatcher.SendAsync(query);
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
    /// <param name="createIssueRequest">Данные о новой задаче</param>
    /// <returns></returns>
    /// <response code="201">Новая задача успешно создана</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPost]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> CreateIssueAsync([FromBody] IssueCreateRequest createIssueRequest)
    {
        IssueCreateCommand createIssueCommand = createIssueRequest.ToCommand();
        IssueResponse response = await dispatcher.SendAsync(createIssueCommand);
        return CreatedAtAction(nameof(GetTaskByIdAsync), new { id = response.Id }, response);
    }

    /// <summary>
    ///     Обновить данные задачи по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     PUT /api/v1/Issues/guid
    ///     {
    ///     }
    /// </remarks>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="updateIssueRequest">Обновленные данные задачи</param>
    /// <returns></returns>
    /// <response code="200">Данные о задаче успешно обновлены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найдена задача для обновления</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> UpdateIssueAsync(Guid id,
        [FromBody] UpdateIssueRequest updateIssueRequest)
    {
        //if (updateIssueRequest is null)
        //{
        //    return Task.FromResult<ActionResult<IssueResponse>>(Problem(type: "BadRequest", title: "Invalid request",
        //        detail: "Некорректный запрос",
        //        statusCode: StatusCodes.Status400BadRequest));
        //}
        IssueUpdateCommand command = IssueRequestToCommandMapping.CreateUpdateCommand(id, updateIssueRequest);
        IssueResponse response = await dispatcher.SendAsync(command);
        return Ok(response);
    }


    /// <summary>
    ///     Удалить задачу по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     DELETE /api/v1/Issues/guid
    /// </remarks>
    /// <param name="id">Идентификатор задачи для удаления</param>
    /// <returns></returns>
    /// <response code="204">Задача успешно удалена</response>
    /// <response code="404">Не найдена задача для удаления</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteIssueAsync(Guid id)
    {
        IssueDeleteByIdCommand command = new(id);
        int response = await dispatcher.SendAsync(command);
        return NoContent();
    }
}
