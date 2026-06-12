using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Features.Attachments;
using TaskService.Application.Features.Attachments.AttachmentDelete;
using TaskService.Application.Features.Attachments.AttachmentDownload;
using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Mediator;

namespace TaskService.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AttachmentsController(IDispatcher dispatcher) : ControllerBase
    {

        /// <summary>
        ///     Скачать файл по идентификатору
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     ```
        ///     GET /api/v1/Attachments/{id}
        ///     ```
        ///     В ответ возвращается бинарный поток файла.
        ///     Заголовок `Content-Disposition: attachment` инициирует скачивание в браузере.
        /// </remarks>
        /// <param name="id">Уникальный идентификатор файла (GUID)</param>
        /// <param name="cancellationToken">Токен отмены запроса</param>
        /// <response code="200">Файл успешно получен. Тело ответа — бинарные данные.</response>
        /// <response code="400">Некорректный формат идентификатора</response>
        /// <response code="404">Файл с указанным идентификатором не найден</response>
        /// <response code="500">Внутренняя ошибка сервера при обращении к хранилищу</response>
        [HttpGet("{id:guid}")]
        [Produces("application/octet-stream")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownloadAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = new AttachmentDownloadQuery(id);
            AttachmentDto response = await dispatcher.SendAsync(query);
            return File(response.Content, response.ContentType, response.Name, enableRangeProcessing: true);
        }

        /// <summary>
        ///     Удалить файл по идентификатору
        /// </summary>
        /// <remarks>
        ///     Пример запроса:
        ///     ```
        ///     DELETE /api/v1/Attachments/{id}
        ///     ```
        /// </remarks>
        /// <param name="id">Уникальный идентификатор файла (GUID)</param>
        /// <param name="cancellationToken">Токен отмены запроса</param>
        /// <response code="204">Файл успешно удален</response>
        /// <response code="400">Некорректный формат идентификатора</response>
        /// <response code="404">Файл с указанным идентификатором не найден</response>
        /// <response code="500">Внутренняя ошибка сервера при обращении к хранилищу</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = new AttachmentDeleteQuery(id);
            await dispatcher.SendAsync(query);
            return NoContent();
        }
    }
}
