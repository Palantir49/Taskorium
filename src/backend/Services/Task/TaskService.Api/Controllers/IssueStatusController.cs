using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Features.Issues.Mapping;
using TaskService.Application.Features.IssueStatuses;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Requests;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.IssueStatus;
using TaskService.Contracts.IssueStatus.Request;

namespace TaskService.Api.Controllers
{
    /// <summary>
    ///     Контроллер для работы с статусами задач
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IssueStatusController(IDispatcher dispatcher) : ControllerBase
    {
        /// <summary>
        ///     Получить данные статуса задачи по Id
        /// </summary>
        /// ///
        /// <remarks>
        ///     Пример запроса:
        ///     GET /api/v1/IssueStatus/guid
        /// </remarks>
        /// <param name="id">Идентификатор статуса задачи</param>
        /// <response code="200">Данные о статусе задачи успешно получены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден статус задачи по заданному id</response>
        [HttpGet("{id:guid}")]
        [ActionName("GetIssueStatusByIdAsync")]
        [ProducesResponseType(typeof(IssueStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueStatusResponse>> GetIssueStatusByIdAsync(Guid id)
        {
            IssueStatusGetByIdQuery query = new IssueStatusGetByIdQuery(id);
            IssueStatusResponse response = await dispatcher.SendAsync(query);
            return Ok(response);
        }

        /// <summary>
        ///     Создать новый статус задачи
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     POST /api/v1/IssueStatus
        ///     {
        ///     }
        /// </remarks>
        /// <param name="issueStatusCreateRequest">Данные о новом статусе задач</param>
        /// <returns></returns>
        /// <response code="201">Новый статус успешно создан</response>
        /// <response code="400">Некорректный запрос</response>
        [HttpPost]
        [ProducesResponseType(typeof(IssueStatusResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueStatusResponse>> CreateIssueStatusAsync([FromBody] IssueStatusCreateRequest issueStatusCreateRequest)
        {
            IssueStatusCreateCommand createIssueCommand = issueStatusCreateRequest.ToCommand();
            IssueStatusResponse response = await dispatcher.SendAsync(createIssueCommand);
            return CreatedAtAction(nameof(GetIssueStatusByIdAsync), new { response.id }, response);
        }

        /// <summary>
        ///     Обновить данные статуса задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     PUT /api/v1/IssueStatus/guid
        ///     {
        ///     }
        /// </remarks>
        /// <param name="id">Идентификатор статуса задачи</param>
        /// <param name="issueStatusUpdateRequest">Обновленные данные статуса задачи</param>
        /// <returns></returns>
        /// <response code="200">Данные о статусе задачи успешно обновлены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден статус задачи для обновления</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(IssueResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueStatusResponse>> UpdateIssueAsync(Guid id,
            [FromBody] IssueStatusUpdateRequest issueStatusUpdateRequest)
        {
            IssueStatusUpdateCommand command = IssueStatusMapping.IssueStatusUpdateCommand(id, issueStatusUpdateRequest);
            IssueStatusResponse response = await dispatcher.SendAsync(command);
            return Ok(response);
        }

        /// <summary>
        ///     Удалить статус задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     DELETE /api/v1/IssuesStatus/guid
        /// </remarks>
        /// <param name="id">Идентификатор статуса задачи для удаления</param>
        /// <returns></returns>
        /// <response code="204">Статус задачи успешно удален</response>
        /// <response code="404">Не найден статус задачи для удаления</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIssueStatusAsync(Guid id)
        {
            IssueStatusDeleteByIdCommand command = new(id);
            int response = await dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}
