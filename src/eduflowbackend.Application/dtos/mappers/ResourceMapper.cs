using eduflowbackend.Core.Resource;

namespace eduflowbackend.Application.dtos.mappers;

public class ResourceMapper
{
    public static ResourceDto ToDTO(Resource resource)
    {
        return new ResourceDto
        {
            Id = resource.Id,
            Title = resource.Title,
            Description = resource.Description,
            CreatedAt = resource.CreatedAt,
            AddedById = resource.CreatorId,
            SessionId = resource.SessionId,
        };
    }

    public static Resource ToEntity(ResourceDto dto, Guid addedById)
    {
        return new Resource
        {
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            CreatorId = addedById,
            SessionId = dto.SessionId
        };
    }
}