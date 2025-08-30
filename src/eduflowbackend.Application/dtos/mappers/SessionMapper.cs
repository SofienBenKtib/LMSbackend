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
            Link= session.Link,
            StartDate = session.StartDate,
            /*InstructorId = session.InstructorId,*/
        };
    }

    public static Session ToEntity(SessionDto dto)
    {
        return new Session
        {
            Title = dto.Title,
            Link = dto.Link,
            StartDate = dto.StartDate,
            InstructorId = dto.InstructorId,
        };
    }
}