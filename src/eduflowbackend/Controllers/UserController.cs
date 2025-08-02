using eduflowbackend.Application.Queries;
using eduflowbackend.Application.Users.Create;
using eduflowbackend.Application.Users.Get;
using eduflowbackend.Core.Exceptions;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace eduflowbackend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddUser([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersCommand());
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        /*var cmd = new UpdateUserCommand
        {
            Id = id,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
        };*/
        var result = await _mediator.Send(command);

        return result.IsFailed ? BadRequest(result.Errors) : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            if (result.IsFailed)
            {
                if (result.HasError<NotFoundError>())
                    return NotFound(result.Errors);
                return BadRequest(result.Errors);
            }

            return NoContent();
            //return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}