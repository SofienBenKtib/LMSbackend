using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eduflowbackend.Application.Resources.Download;

public record DownloadResourceCommand(Guid ResourceId) : IRequest<ResourceDownloadResult>;

public record ResourceDownloadResult(byte[] FileData, string ContentType, string FileName);