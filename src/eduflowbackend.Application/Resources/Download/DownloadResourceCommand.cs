using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eduflowbackend.Application.Resources.Download;

public record DownloadResourceCommand(Guid ResourceId) : IRequest<ResourceDownloadResult>;

public class ResourceDownloadResult
{
    public byte[] FileContent { get; set; }
    public string FileName { get; set; }
}