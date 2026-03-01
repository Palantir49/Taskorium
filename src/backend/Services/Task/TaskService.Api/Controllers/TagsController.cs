using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Features.IssueStatuses;
using TaskService.Application.Features.Tags;
using TaskService.Application.Features.Tags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Tag;
using TaskService.Contracts.Tag.Request;

namespace TaskService.Api.Controllers
{
    /// <summary>
    ///     Контроллер для работы с типами задач
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TagsController(IDispatcher dispatcher) : ControllerBase
    {
        /// <summary>
        ///     Получить данные типа задачи по Id
        /// </summary>
        /// ///
        /// <remarks>
        ///     Пример запроса:
        ///     GET /api/v1/Tags/guid
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи</param>
        /// <response code="200">Данные о типа задачи успешно получены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден тип задачи по заданному id</response>
        [HttpGet("{id:guid}")]
        [ActionName("GetTagByIdAsync")]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TagResponse>> GetTagByIdAsync(Guid id)
        {
            TagGetByIdQuery query = new TagGetByIdQuery(id);
            TagResponse response = await dispatcher.SendAsync(query);
            return Ok(response);
        }

        /// <summary>
        ///     Создать новый тип задачи
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     POST /api/v1/Tags
        ///     {
        ///     }
        /// </remarks>
        /// <param name="TagCreateRequest">Данные о новом типе задач</param>
        /// <returns></returns>
        /// <response code="201">Новый тип задачи успешно создан</response>
        /// <response code="400">Некорректный запрос</response>
        [HttpPost]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TagResponse>> CreateTagAsync([FromBody] TagCreateRequest TagCreateRequest)
        {
            TagCreateCommand createIssueCommand = TagCreateRequest.ToCommand();
            TagResponse response = await dispatcher.SendAsync(createIssueCommand);
            return CreatedAtAction(nameof(GetTagByIdAsync), new { response.id }, response);
        }

        /// <summary>
        ///     Обновить данные типа задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     PUT /api/v1/Tags/guid
        ///     {
        ///     }
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи</param>
        /// <param name="TagUpdateRequest">Обновленные данные типа задачи</param>
        /// <returns></returns>
        /// <response code="200">Данные о типе задачи успешно обновлены</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найден тип задачи для обновления</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TagResponse>> UpdateTagsAsync(Guid id,
            [FromBody] TagUpdateRequest TagUpdateRequest)
        {
            TagUpdateCommand command = TagMapping.TagUpdateCommand(id, TagUpdateRequest);
            TagResponse response = await dispatcher.SendAsync(command);
            return Ok(response);
        }

        /// <summary>
        ///     Удалить тип задачи по Id
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     DELETE /api/v1/Tags/guid
        /// </remarks>
        /// <param name="id">Идентификатор типа задачи для удаления</param>
        /// <returns></returns>
        /// <response code="204">Тип задачи успешно удален</response>
        /// <response code="404">Не найден тип задачи для удаления</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTagAsync(Guid id)
        {
            TagDeleteByIdCommand command = new(id);
            int response = await dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}
