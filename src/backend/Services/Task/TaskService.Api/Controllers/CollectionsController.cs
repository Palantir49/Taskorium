using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Features.Collections.Query;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.Collections;
using TaskService.Contracts.IssueStatus;

namespace TaskService.Api.Controllers
{
    /// <summary>
    ///     Контроллер для получения списков коллекций
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CollectionsController(IDispatcher dispatcher) : Controller
    {
        /// <summary>
        ///     Получить список типов задач
        /// </summary>
        /// ///
        /// <remarks>
        ///     Пример запроса:
        ///     GET /api/v1/Collections/IssueType
        /// </remarks>
        [HttpGet("IssueType")]
        [ProducesResponseType(typeof(IssueStatusResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IssueStatusResponse>> GetIssueTypeAsync()
        {
            GetAllIsssueTypesQuery query = new();
            IEnumerable<IssueTypeResponse> response = await dispatcher.SendAsync(query);
            return Ok(response);
        }

        /// <summary>
        ///     Получить список типов задач
        /// </summary>
        /// ///
        /// <remarks>
        ///     Пример запроса:
        ///     GET /api/v1/Collections/IssueStatus
        /// </remarks>
        [HttpGet("IssueStatusType")]
        [ProducesResponseType(typeof(IssueStatusResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IssueStatusResponse>> GetIssueStatusTypeAsync()
        {
            GetAllIsssueTypesQuery query = new();
            IEnumerable<IssueTypeResponse> response = await dispatcher.SendAsync(query);
            return Ok(response);
        }
    }
}
