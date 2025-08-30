using eduflowbackend.Application.Sessions.Create;
using eduflowbackend.Application.Sessions.Delete;
using eduflowbackend.Application.Sessions.Get;
using eduflowbackend.Core.Session;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace eduflowbackend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionCommand request)
    {
        try
        {
            var command = new CreateSessionCommand(request.Link);
            var session = await _mediator.Send(command);
            return Ok(session);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSessionById(Guid id)
    {
        var result = await _mediator.Send(new GetSessionByIdQuery(id));
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Session>>> GetAllSessions()
    {
        var query = new GetAllSessionsQuery();
        var sessions = await _mediator.Send(query);
        return Ok(sessions);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSessionById(Guid id)
    {
        var result = await _mediator.Send(new DeleteSessionCommand(id));
        return Ok(result);
    }
}