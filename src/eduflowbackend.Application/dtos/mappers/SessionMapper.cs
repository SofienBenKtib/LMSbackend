using eduflowbackend.Core.Session;

namespace eduflowbackend.Application.dtos.mappers;

public class SessionMapper
{
    public static SessionDto ToDto(Session session)
    {
        return new SessionDto
        {
            Id = session.Id,
            Title = session.Title,
            Description = session.Description,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            InstructorId = session.InstructorId,
        };
    }

    public static Session ToEntity(SessionDto dto)
    {
        return new Session
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            InstructorId = dto.InstructorId,
        };
    }
}