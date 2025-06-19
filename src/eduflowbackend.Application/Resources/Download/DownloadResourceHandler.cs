using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace eduflowbackend.Application.Resources.Download;

public class DownloadResourceHandler : IRequestHandler<DownloadResourceCommand, ResourceDownloadResult>
{
    private readonly IRepository<Resource> _repository;

    public DownloadResourceHandler(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<ResourceDownloadResult> Handle(DownloadResourceCommand request,
        CancellationToken cancellationToken)
    {
        //  Retrieves the resource from the repository
        var resource = await _repository.GetByIdAsync(request.ResourceId, cancellationToken);
        if (resource == null)
            throw new FileNotFoundException("Resource not found");

        //  Determining the file extension from the title
        var extension = Path.GetExtension(resource.Title);
        var storedFileName = resource.Id + extension;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", storedFileName);

        if (!File.Exists(path))
            throw new FileNotFoundException("File not found");

        var bytes = await File.ReadAllBytesAsync(path, cancellationToken);
        return new ResourceDownloadResult
        {
            FileContent = bytes,
            FileName = resource.Title
        };
    }
}