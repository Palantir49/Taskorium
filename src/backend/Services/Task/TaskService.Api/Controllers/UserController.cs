using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Features.Users;
using TaskService.Application.Features.Users.Read.GetUserById;
using TaskService.Application.Features.Users.Read.GetUserByKeycloakId;
using TaskService.Application.Features.Users.Read.GetUserWorkspacesById;
using TaskService.Application.Features.Users.Read.GetUsesrPage;
using TaskService.Application.Features.Users.Write.CreateUser;
using TaskService.Application.Features.Users.Write.DeleteUserById;
using TaskService.Application.Features.Users.Write.UpdateUserEmail;
using TaskService.Application.Mediator;
using TaskService.Contracts.User.Requests;
using TaskService.Contracts.User.Responses;

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
    [HttpGet("{id:guid}")]
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
    ///     Получить рабочие области пользователя по  id
    /// </summary>
    /// ///
    /// <param name="Id">KeycloakId пользователя</param>
    [HttpGet("{id:guid}/workspaces")]
    [ActionName("GetUserWorkspacesByIdAsync")]
    [ProducesResponseType(typeof(List<UsersWorkspaceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<List<UsersWorkspaceResponse>>> GetUserWorkspacesByIdAsync(Guid Id)
    {
        var result = await dispatcher.SendAsync(new GetUserWorkspacesByIdQuery(Id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.UsersWorkspaces);
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
    [HttpGet("users")]
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
    /// <param name="request">Данные нового пользователя</param>
    /// <returns></returns>
    /// <response code="201">Новый пользователь успешно создан</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateUserResult>> CreateUserAsync(
        [FromBody] CreateUserRequest request)
    {
        var userCommand = request.ToCommand();
        var response = await dispatcher.SendAsync(userCommand);
        return CreatedAtAction(nameof(GetUserByIdAsync), new { response.Id }, response);
    }

    /// <summary>
    ///     Удаление пользователя по id
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
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
    /// <param name="id">Id пользователя</param>
    /// <param name="email">Новое значение email</param>
    /// <returns></returns>
    /// <response code="201">Email пользователя успешно обновлен</response>
    /// <response code="400">Некорректный запрос</response>
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<UpdateUserEmailResult>> UpdateUserEmailAsync(Guid id,
        [FromBody] string email)
    {
        var command = new UpdateUserEmailCommand(id, email);
        var response = await dispatcher.SendAsync(command);
        return Ok(response);
    }
}
