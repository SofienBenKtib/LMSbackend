using eduflowbackend.Core.User;

namespace eduflowbackend.Application.dtos.mappers;

public class UserMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.PhoneNumber
        };
    }
}