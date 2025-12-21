using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Interfaces;
using TaskService.Contracts.Issue.Requests;
using TaskService.Contracts.Issue.Responses;
using TaskService.Contracts.Task.Requests;
using TaskService.Contracts.Task.Responses;

namespace TaskService.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskController : Controller
{
    private IIssueService _taskService;

    public TaskController(IIssueService taskService)
    {
        _taskService = taskService; 
    }

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
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TaskResponse>> GetTaskByIdAsync(Guid id)
    {
        var taskResponse = await _taskService.GetTaskByIdAsync(id);
        if (taskResponse == null)
            return NotFound();

        return Ok(taskResponse);
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
    /// <param name="createTaskRequest">Данные о новой задаче</param>
    /// <returns></returns>
    /// <response code="201">Новая задача успешно создана</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TaskResponse>> CreateIssueAsync([FromBody] CreateTaskRequest createTaskRequest)
    {
        var taskResponse = await _taskService.CreateTaskAsync(createTaskRequest);
        return CreatedAtAction(nameof(GetTaskByIdAsync), new { id = taskResponse.id }, taskResponse);
    }
}
