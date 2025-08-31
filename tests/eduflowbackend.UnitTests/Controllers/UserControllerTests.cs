using eduflowbackend.Application.Queries;
using eduflowbackend.Controllers;
using Moq;
using Xunit;
using FluentResults;
using Mediator;
using Microsoft.AspNetCore.Mvc;

public class UserControllerTests
{
    [Fact]
    public async Task DeleteUser_Success_ReturnsNoContent()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        
        // Mock SUCCESS result - Result<Guid>.Ok with the user ID
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result.Ok(Guid.NewGuid())); // Returns successful Result<Guid>
        
        var controller = new UserController(mediatorMock.Object);
        var userId = Guid.NewGuid();

        // Act
        var result = await controller.DeleteUser(userId);

        // Assert - Should return NoContent (204) on success
        Assert.IsType<NoContentResult>(result);
    }
    

    [Fact]
    public async Task DeleteUser_OtherError_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        
        // Mock FAILURE result - Other error
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result.Fail<Guid>("Some other error")); // Returns failed Result<Guid>
        
        var controller = new UserController(mediatorMock.Object);
        var userId = Guid.NewGuid();

        // Act
        var result = await controller.DeleteUser(userId);

        // Assert - Should return BadRequest (400) for other errors
        Assert.IsType<BadRequestObjectResult>(result);
    }
}

// Simple test version of NotFoundError
public class NotFoundError : IError
{
    public string Message { get; }
    public Dictionary<string, object> Metadata { get; } = new();
    
    public NotFoundError(string entity, string id)
    {
        Message = $"{entity} with ID {id} not found";
    }

    public List<IError> Reasons { get; }
}