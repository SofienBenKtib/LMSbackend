using eduflowbackend.Application.Sessions.Create;
using eduflowbackend.Application.Sessions.Delete;
using eduflowbackend.Application.Sessions.Get;
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
    public async Task<IActionResult> AddSession(CreateSessionCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSessionById(Guid id)
    {
        var result = await _mediator.Send(new GetSessionByIdQuery(id));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSessions()
    {
        var result = await _mediator.Send(new GetAllSessionsQuery());
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSessionById(Guid id)
    {
        var result = await _mediator.Send(new DeleteSessionCommand(id));
        return Ok(result);
    }
}