//using Microsoft.AspNetCore.Mvc;
//using TaskService.Contracts.User.Requests;
//using TaskService.Contracts.User.Responses;
//using TaskService.Application.Commands.Users.Create;
//using TaskService.Application.Commands.Users;
//using TaskService.Application.Commands.Users.Get;

//namespace TaskService.Api.Controllers;

///// <summary>
/////     Контроллер для работы с рабочими пространствами
///// </summary>
//// <param name="createUserHandler">Handler создания пользователя</param>
//// <param name="getUserHandler">Handler получения пользователя по Id</param>
//[ApiController]
//[Route("api/v1/[controller]")]
//public class UserController : Controller
//{
//    /// <summary>
//    ///     Получить пользователя по Id
//    /// </summary>
//    /// ///
//    /// <remarks>
//    ///     Пример запроса:
//    ///     GET /api/v1/User/
//    ///     {
//    ///     }
//    /// </remarks>
//    /// <response code="200">Данные о рабочей области успешно получены</response>
//    /// <response code="400">Некорректный запрос</response>
//    /// <response code="404">Не найдена рабочая область по заданному id</response>
//    [HttpGet]
//    [ActionName("GetUserByIdAsync")]
//    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
//    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(StatusCodes.Status409Conflict)]
//    public async Task<ActionResult<UserResponse>> GetUserByIdAsync([FromBody] GetUserRequest request)
//    {

//        var userResponse = await getUserHandler.HandleAsync(request.ToCommand());
//        if (userResponse == null)
//        {
//            return NotFound();
//        }

//        return Ok(userResponse);
//    }

//    /// <summary>
//    ///     Создать нового пользователя
//    /// </summary>
//    /// <remarks>
//    ///     Пример запроса:
//    ///     POST /api/v1/User
//    ///     {
//    ///     }
//    /// </remarks>
//    /// <param name="createUserRequest">Данные нового пользователя</param>
//    /// <returns></returns>
//    /// <response code="201">Новый пользователь успешно создан</response>
//    /// <response code="400">Некорректный запрос</response>
//    [HttpPost]
//    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
//    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(StatusCodes.Status409Conflict)]
//    public async Task<ActionResult<UserResponse>> CreateUserAsync(
//        [FromBody] CreateUserRequest createUserRequest)
//    {
//        var workspaceResponse = await createUserHandler.HandleAsync(createUserRequest.ToCommand());
//        return CreatedAtAction(nameof(CreateUserAsync), new { id = workspaceResponse.id }, workspaceResponse);
//    }

//    ///// <summary>
//    /////     Удалить задачу по Id
//    ///// </summary>
//    ///// <remarks>
//    /////     Пример запроса:
//    /////     DELETE /api/v1/Issues/1
//    ///// </remarks>
//    ///// <param name="id">Идентификатор задачи для удаления</param>
//    ///// <returns></returns>
//    ///// <response code="204">Задача успешно удалена</response>
//    ///// <response code="404">Не найдена задача для удаления</response>
//    //[HttpDelete("{id:guid}")]
//    //[ProducesResponseType(StatusCodes.Status204NoContent)]
//    //[ProducesResponseType(StatusCodes.Status404NotFound)]
//    //public Task<IActionResult> DeleteWorkspaceAsync(Guid id)
//    //{
//    //    return Task.FromResult<IActionResult>(NoContent());
//    //}
//}
