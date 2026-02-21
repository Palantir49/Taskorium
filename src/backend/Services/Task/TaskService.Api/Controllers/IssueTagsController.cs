using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Features.IssueStatuses;
using TaskService.Application.Features.IssueTags;
using TaskService.Application.Features.IssueTags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Contracts.IssueTag;
using TaskService.Contracts.IssueTag.Request;

namespace TaskService.Api.Controllers
{
    /// <summary>
    ///     Контроллер для работы с типами задач
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IssueTagsController(IDispatcher dispatcher) : ControllerBase
    {
        /// <summary>
        ///     Получить данные типа задачи по Id
        /// </summary>
        /// ///
        /// <remarks>
        ///     Пример запроса:
        ///     GET /api/v1/IssueTags/guid
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи</param>
        /// <response code="200">Данные о типа задачи успешно получены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден тип задачи по заданному id</response>
        [HttpGet("{id:guid}")]
        [ActionName("GetIssueTagByIdAsync")]
        [ProducesResponseType(typeof(IssueTagResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueTagResponse>> GetIssueTagByIdAsync(Guid id)
        {
            IssueTagGetByIdQuery query = new IssueTagGetByIdQuery(id);
            IssueTagResponse response = await dispatcher.SendAsync(query);
            return Ok(response);
        }

        /// <summary>
        ///     Создать новый тип задачи
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     POST /api/v1/IssueTags
        ///     {
        ///     }
        /// </remarks>
        /// <param name="issueTagCreateRequest">Данные о новом типе задач</param>
        /// <returns></returns>
        /// <response code="201">Новый тип задачи успешно создан</response>
        /// <response code="400">Некорректный запрос</response>
        [HttpPost]
        [ProducesResponseType(typeof(IssueTagResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueTagResponse>> CreateIssueTagAsync([FromBody] IssueTagCreateRequest issueTagCreateRequest)
        {
            IssueTagCreateCommand createIssueCommand = issueTagCreateRequest.ToCommand();
            IssueTagResponse response = await dispatcher.SendAsync(createIssueCommand);
            return CreatedAtAction(nameof(GetIssueTagByIdAsync), new { response.id }, response);
        }

        /// <summary>
        ///     Обновить данные типа задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     PUT /api/v1/IssueTags/guid
        ///     {
        ///     }
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи</param>
        /// <param name="issueTagUpdateRequest">Обновленные данные типа задачи</param>
        /// <returns></returns>
        /// <response code="200">Данные о типе задачи успешно обновлены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден тип задачи для обновления</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(IssueStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IssueStatusResponse>> UpdateIssueTagsAsync(Guid id,
            [FromBody] IssueTagUpdateRequest issueTagUpdateRequest)
        {
            IssueTagUpdateCommand command = IssueTagMapping.IssueTagUpdateCommand(id, issueTagUpdateRequest);
            IssueTagResponse response = await dispatcher.SendAsync(command);
            return Ok(response);
        }

        /// <summary>
        ///     Удалить тип задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     DELETE /api/v1/IssueTags/guid
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи для удаления</param>
        /// <returns></returns>
        /// <response code="204">Тип задачи успешно удален</response>
        /// <response code="404">Не найден тип задачи для удаления</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIssueTagAsync(Guid id)
        {
            IssueTagDeleteByIdCommand command = new(id);
            int response = await dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}
