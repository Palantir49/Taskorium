using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Features.IssueStatuses;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Features.IssueTypes;
using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.IssueStatus;
using TaskService.Contracts.IssueStatus.Request;
using TaskService.Contracts.IssueType;
using TaskService.Contracts.IssueType.Request;

namespace TaskService.Api.Controllers
{
    /// <summary>
    ///     Контроллер для работы с типами задач
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IssueTypesController(IDispatcher dispatcher) : ControllerBase
    {
        /// <summary>
        ///     Получить данные типа задачи по Id
        /// </summary>
        /// ///
        /// <remarks>
        ///     Пример запроса:
        ///     GET /api/v1/IssueTypes/guid
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи</param>
        /// <response code="200">Данные о типа задачи успешно получены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден тип задачи по заданному id</response>
        [HttpGet("{id:guid}")]
        [ActionName("GetIssueTypesByIdAsync")]
        [ProducesResponseType(typeof(IssueTypeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueTypeResponse>> GetIssueTypeByIdAsync(Guid id)
        {
            IssueTypeGetByIdQuery query = new IssueTypeGetByIdQuery(id);
            IssueTypeResponse response = await dispatcher.SendAsync(query);
            return Ok(response);
        }

        /// <summary>
        ///     Создать новый тип задачи
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     POST /api/v1/IssueTypes
        ///     {
        ///     }
        /// </remarks>
        /// <param name="issueTypeCreateRequest">Данные о новом типе задач</param>
        /// <returns></returns>
        /// <response code="201">Новый тип задачи успешно создан</response>
        /// <response code="400">Некорректный запрос</response>
        [HttpPost]
        [ProducesResponseType(typeof(IssueTypeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueTypeResponse>> CreateIssueTypeAsync([FromBody] IssueTypeCreateRequest issueTypeCreateRequest)
        {
            IssueTypeCreateCommand createIssueCommand = issueTypeCreateRequest.ToCommand();
            IssueTypeResponse response = await dispatcher.SendAsync(createIssueCommand);
            return CreatedAtAction(nameof(GetIssueTypeByIdAsync), new { response.id }, response);
        }

        /// <summary>
        ///     Обновить данные типа задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     PUT /api/v1/IssueTypes/guid
        ///     {
        ///     }
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи</param>
        /// <param name="issueTypeUpdateRequest">Обновленные данные типа задачи</param>
        /// <returns></returns>
        /// <response code="200">Данные о типе задачи успешно обновлены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден тип задачи для обновления</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(IssueStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueStatusResponse>> UpdateIssueStatusAsync(Guid id,
            [FromBody] IssueTypeUpdateRequest issueTypeUpdateRequest)
        {
            IssueTypeUpdateCommand command = IssueTypeMapping.IssueTypeUpdateCommand(id, issueTypeUpdateRequest);
            IssueTypeResponse response = await dispatcher.SendAsync(command);
            return Ok(response);
        }

        /// <summary>
        ///     Удалить тип задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     DELETE /api/v1/IssueTypes/guid
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи для удаления</param>
        /// <returns></returns>
        /// <response code="204">Тип задачи успешно удален</response>
        /// <response code="404">Не найден тип задачи для удаления</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIssueTypeAsync(Guid id)
        {
            IssueTypeDeleteByIdCommand command = new(id);
            int response = await dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}
