﻿using eduflowbackend.Application.Resources.Create;
using eduflowbackend.Application.Resources.Delete;
using eduflowbackend.Application.Resources.Download;
using eduflowbackend.Application.Resources.Get;
using eduflowbackend.Application.Resources.Update;
using eduflowbackend.Application.Resources.Upload;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace eduflowbackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRepository<Resource> _repository;

    public ResourcesController(IMediator mediator, IRepository<Resource> repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> AddResource(CreateResourceCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadResource([FromBody] IFormFile file)
    {
        try
        {
            //  Creating the command
            var command = new UploadResourceCommand(file);
            //  Sending the command through IMediatR
            var filedId = await _mediator.Send(command, HttpContext.RequestAborted);

            return Ok(filedId);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadResource(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DownloadResourceCommand(id), cancellationToken);
        return File(result.FileContent, result.FileName);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetResourceById(Guid id)
    {
        var result = await _mediator.Send(new GetResourceByIdQuery(id));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllResources()
    {
        var result = await _mediator.Send(new GetAllResourcesQuery());
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResource(Guid id, UpdateResourceCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResource(Guid id)
    {
        var result = await _mediator.Send(new DeleteResourceCommand(id));
        return Ok(result);
    }
}