using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Commands.Users;
using TaskService.Application.Commands.Users.Get;
using TaskService.Application.Features.Users.Create;
using TaskService.Application.Features.Users.Delete;
using TaskService.Application.Features.Users.Get;
using TaskService.Application.Features.Users.Update;
using TaskService.Application.Features.Workspaces.Update;
using TaskService.Application.Mediator;
using TaskService.Contracts.User.Requests;
using TaskService.Contracts.User.Responses;
using TaskService.Contracts.Workspace.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskService.Api.Controllers;

/// <summary>
///     Контроллер для работы с рабочими пространствами
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UserController(IDispatcher dispatcher) : Controller
{
    /// <summary>
    ///     Получить пользователя по Id
    /// </summary>
    /// ///
    /// <param name="id">Id пользователя</param>
    [HttpGet]
    [ActionName("GetUserByIdAsync")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GetUserByIdResult>> GetUserByIdAsync(Guid id)
    {

        var userResponse = await dispatcher.SendAsync(new GetUserByIdQuery(id));
        if (userResponse == null)
        {
            return NotFound();
        }

        return Ok(userResponse);
    }
    /// <summary>
    ///     Получить пользователя по keycloak id
    /// </summary>
    /// ///
    /// <param name="id">Id пользователя</param>
    [HttpGet("GetUserByKeycloakId")]
    [ActionName("GetUserByKeycloakIdAsync")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GetUserByIdResult>> GetUserByKeycloakIdAsync(Guid id)
    {

        var userResponse = await dispatcher.SendAsync(new GetUserByKeycloakIdQuery(id));
        if (userResponse == null)
        {
            return NotFound();
        }

        return Ok(userResponse);
    }
    /// <summary>
    ///     Получить список пользователей
    /// </summary>
    /// ///
    /// <param name="query">Объект пагинации</param>
    [HttpGet("GetAllUsers")]
    [ActionName("GetAllUsersAsync")]
    [ProducesResponseType(typeof(GetUsersPageResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GetUsersPageResult>> GetAllUsersAsync([FromQuery] GetUsersPageQuery query)
    {

        var response = await dispatcher.SendAsync(query);
        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }
    /// <summary>
    ///     Создать нового пользователя
    /// </summary>
    /// <param name="command">Данные нового пользователя</param>
    /// <returns></returns>
    /// <response code="201">Новый пользователь успешно создан</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateUserResult>> CreateUserAsync(
        [FromBody] CreateUserCommand command)
    {
        var response = await dispatcher.SendAsync(command);
        return CreatedAtAction(nameof(GetUserByIdAsync), new { response.id }, response);
    }

    /// <summary>
    ///     Удаление пользователя по id
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DeleteUserByIdResult>> DeleteUserAsync(
        Guid id)
    {
        var response = await dispatcher.SendAsync(new DeleteUserByIdCommand(id));
        return Ok(response);
    }

    /// <summary>
    ///     Обновление логина пользователя
    /// </summary>
    /// <param name="command">Id рабочей области и имя рабочей области</param>
    /// <returns></returns>
    /// <response code="201">Имя рабочей области успешно обновлено</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPatch]
    public async Task<ActionResult<UpdateUserEmailResult>> UpdateUserEmailAsync(
        [FromBody] UpdateUserEmailCommand command)
    {
        var response = await dispatcher.SendAsync(command);
        return Ok(response);
    }
}
