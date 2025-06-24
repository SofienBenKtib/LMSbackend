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
        // //  Retrieves the resource from the repository
        // var resource = await _repository.GetByIdAsync(request.ResourceId, cancellationToken);
        // if (resource == null)
        //     throw new FileNotFoundException("Resource not found");

        //  Locate the uploads directory
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        //  Finding a matching file based on the id
        var files = Directory.GetFiles(uploadPath, request.ResourceId + ".*");
        if (files.Length == 0)
            throw new FileLoadException("File not found");

        var filePath = files[0];
        var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);

        var extension = Path.GetExtension(filePath);
        var fileName = Path.GetFileName(filePath);
        var contentType = GetContentType(extension);

        return new ResourceDownloadResult(fileBytes, contentType, fileName);
    }

    private static string GetContentType(string extension) =>
        extension.ToLower()switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".txt" => "text/plain",
            _ => "text/plain",
        };
}