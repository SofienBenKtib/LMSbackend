using eduflowbackend.Core.User;

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
            Phone = user.PhoneNumber
        };
    }
}