using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eduflowbackend.Application.Resources.Download;

public class DownloadResourceCommand : IRequest<Guid>
{
    public Guid ResourceId { get; set; }

    public DownloadResourceCommand(Guid resourceId)
    {
        ResourceId = resourceId;
    }
}