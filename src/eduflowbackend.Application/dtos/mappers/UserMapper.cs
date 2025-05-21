using eduflowbackend.Core.entities;

namespace eduflowbackend.Application.dtos.mappers;

public class UserMapper
{
    public static UserDTO ToDto(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            CreatedOn = user.CreatedOn,
            ModifiedOn = user.ModifiedOn,
        };
    }

    public static User ToEntity(UserDTO dto)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password,
            Phone = dto.Phone,
            Role = dto.Role,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }
}