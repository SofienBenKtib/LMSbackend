using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace eduflowbackend.Application.Resources.Download;

public class DownloadResourceHandler : IRequestHandler<DownloadResourceCommand, Guid>
{
    private readonly IRepository<Resource> _repository;

    public DownloadResourceHandler(
        IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Guid> Handle(DownloadResourceCommand request, CancellationToken cancellationToken)
    {
        //  Retrieves the resource from the repository
        var resource = await _repository.GetByIdAsync(request.ResourceId, cancellationToken);
        if (resource == null)
            throw new KeyNotFoundException("Resource not found");

        //  Build the expected file path 
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", resource.Title);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found");
        return resource.Id;
    }
}