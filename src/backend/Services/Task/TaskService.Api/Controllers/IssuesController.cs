using Microsoft.AspNetCore.Mvc;
using TaskService.Contracts.Issue.Requests;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Api.Controllers;

/// <summary>
///     Tasks
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class IssuesController : Controller
{
    /// <summary>
    ///     Получить данные задачи по Id
    /// </summary>
    /// /// <remarks>
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
        return Ok(new IssueResponse
        {
            Id = Guid.CreateVersion7(),
            Key = "DEV-123",
            Summary = "Test"
        });
    }

    /// <summary>
    ///     Создать новою задачу
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     POST /api/v1/Issues
    ///     {
    ///  
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
    public async Task<ActionResult<IssueResponse>> CreateIssueAsync([FromBody] CreateIssueRequest createIssueRequest)
    {

        var issueResponse = new IssueResponse
        {
            Id = Guid.CreateVersion7(),
            Key = "DEV-123",
            Summary = "Test"
        };
        return CreatedAtAction(nameof(GetTaskByIdAsync), new { id = issueResponse.Id }, issueResponse);
    }


    /// <summary>
    ///     Обновить данные задачи по Id
    /// </summary>
    /// <remarks>
    ///     Пример запроса:
    ///     PUT /api/v1/Issues/guid
    ///     {
    ///     
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
        [FromBody] UpdateIssueRequest? updateIssueRequest)
    {
        if (updateIssueRequest is null)
            return Problem(type: "BadRequest", title: "Invalid request", detail: "Некорректный запрос",
                statusCode: StatusCodes.Status400BadRequest);
        var response = new IssueResponse
        {
            Id = Guid.CreateVersion7(),
            Key = "DEV-123",
            Summary = "Test"
        };
        return Ok(response);
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
    public async Task<IActionResult> DeleteIssueAsync(Guid id)
    {
        return NoContent();
    }

}
