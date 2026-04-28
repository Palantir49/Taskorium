using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Policy = "CanViewTask")]
    [HttpGet("{id:guid}")]
    [ActionName("GetTaskByIdAsync")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> GetTaskByIdAsync([FromRoute] Guid id)
    {
        var query = new IssueGetByIdQuery(id);
        var response = await dispatcher.SendAsync(query);
        return Ok(response);
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
    /// <param name="issueId">Идентификатор задачи</param>
    /// <param name="updateIssueRequest">Обновленные данные задачи</param>
    /// <returns></returns>
    /// <response code="200">Данные о задаче успешно обновлены</response>
    /// <response code="400">Некорректный запрос</response>
    /// <response code="404">Не найдена задача для обновления</response>
    [Authorize(Policy = "CanUpdateTask")]
    [HttpPut("{issueId:guid}")]
    [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IssueResponse>> UpdateIssueAsync([FromRoute] Guid issueId,
        [FromBody] UpdateIssueRequest updateIssueRequest)
    {
        //if (updateIssueRequest is null)
        //{
        //    return Task.FromResult<ActionResult<IssueResponse>>(Problem(type: "BadRequest", title: "Invalid request",
        //        detail: "Некорректный запрос",
        //        statusCode: StatusCodes.Status400BadRequest));
        //}
        var command = IssueRequestToCommandMapping.CreateUpdateCommand(issueId, updateIssueRequest);
        var response = await dispatcher.SendAsync(command);
        return Ok(response);
    }


    /// <summary>
    ///     Удалить задачу по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     DELETE /api/v1/Issues/guid
    /// </remarks>
    /// <param name="issueId">Идентификатор задачи для удаления</param>
    /// <returns></returns>
    /// <response code="204">Задача успешно удалена</response>
    /// <response code="404">Не найдена задача для удаления</response>
    [Authorize(Policy = "CanDeleteTask")]
    [HttpDelete("{issueId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteIssueAsync([FromRoute] Guid issueId)
    {
        IssueDeleteByIdCommand command = new(issueId);
        var response = await dispatcher.SendAsync(command);
        return NoContent();
    }

    /// <summary>
    ///     Добавить таг к задаче
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     POST /api/v1/Issues/guid/Tags
    /// </remarks>
    /// <param name="issueId">Идентификатор задачи для удаления</param>
    /// <param name="request">тело запроса с идентификатором тага</param>
    /// <returns></returns>
    /// <response code="204">Связь задачи и тага успешно создана</response>
    /// <response code="404">Не найдена задача или таг для добавления тага</response>
    /// <response code="409">Задача и таг относятся к разынм проектам</response>
    [HttpPost("{issueId:guid}/Tags")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddTagToIssueAsync([FromRoute] Guid issueId, [FromBody] AddTagToIssueRequest request)
    {
        AddTagToIssueCommand command = new(issueId, request.TagId);
        await dispatcher.SendAsync(command);
        return NoContent();
    }


    /// <summary>
    ///     удалить таг у задаче
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     DELETE /api/v1/Issues/guid/Tags/guid
    /// </remarks>
    /// <param name="issueId">Идентификатор задачи для удаления</param>
    /// <param name="tagId">идентификатор тага</param>
    /// <returns></returns>
    /// <response code="204">Связь задачи и тага успешно удалена</response>
    /// <response code="404">Не найдена задача или таг для удаления связи</response>
    [HttpPost("{issueId:guid}/Tags/{tagId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTagToIssueAsync([FromRoute] Guid issueId, [FromRoute] Guid tagId)
    {
        //AddTagToIssueCommand command = new(id, request.TagId);
        //await dispatcher.SendAsync(command);
        return NoContent();
    }
}
