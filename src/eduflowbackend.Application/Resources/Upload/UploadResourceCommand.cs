using Mediator;
using Microsoft.AspNetCore.Http;

namespace eduflowbackend.Application.Resources.Upload;

public record UploadResourceCommand : IRequest<Guid>
{
    public IFormFile File { get; init; }
    public string? Tile { get; init; }
    public string? Description { get; init; }

    public UploadResourceCommand(IFormFile file, string? tile, string? description)
    {
        File = file;
        Tile = tile;
        Description = description;
    }

    public UploadResourceCommand(IFormFile file)
    {
        File = file;
    }
}