using eduflowbackend.Application.Queries;
using eduflowbackend.Application.Users.Update;
using eduflowbackend.Controllers;
using Moq;
using Xunit;
using FluentResults;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using CoreNotFoundError = eduflowbackend.Core.Exceptions.NotFoundError;

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

    // -----------------------------------------------------------------------
    // Regression tests added below.
    // -----------------------------------------------------------------------

    [Fact]
    public async Task DeleteUser_NotFoundError_ReturnsNotFound()
    {
        // Regression test: covers the previously-untested NotFound branch of
        // UserController.DeleteUser. When the DeleteUserHandler reports a
        // Core.Exceptions.NotFoundError (the same type produced by the real
        // handler when the user does not exist), the controller MUST return a
        // 404 NotFound response rather than 400 BadRequest or 204 NoContent.
        //
        // Note: the pre-existing test file defined a local stub "NotFoundError"
        // that is unrelated to Core.Exceptions.NotFoundError, so the NotFound
        // path was effectively unexercised. This test pins it down using the
        // real Core error type (aliased as CoreNotFoundError).

        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var userId = Guid.NewGuid();

        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result.Fail<Guid>(new CoreNotFoundError("User", userId.ToString())));

        var controller = new UserController(mediatorMock.Object);

        // Act
        var result = await controller.DeleteUser(userId);

        // Assert - Should return NotFound (404) when the user does not exist
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteUser_MediatorThrows_ReturnsBadRequestWithMessage()
    {
        // Regression test / edge case: the DeleteUser action wraps the mediator
        // call in a try/catch that turns any exception into a 400 BadRequest
        // with the exception message as the body. This test locks that
        // contract in so an accidental change (e.g. letting the exception
        // bubble up as a 500, or returning a different body shape) is caught.

        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new InvalidOperationException("boom"));

        var controller = new UserController(mediatorMock.Object);

        // Act
        var result = await controller.DeleteUser(Guid.NewGuid());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("boom", badRequest.Value);
    }

    [Fact]
    public async Task UpdateUser_WhenHandlerReturnsFailedResult_ReturnsOk_KnownBug()
    {
        // Regression test reproducing a bug in UserController.UpdateUser:
        //
        //     var result = await _mediator.Send(cmd);
        //     return Ok(result);
        //
        // The controller unconditionally wraps the mediator's Result in an
        // Ok(...) response, even when result.IsFailed is true (for example
        // when UpdateUserHandler reports "User not found"). The correct
        // behavior would be to inspect the Result and return NotFound/
        // BadRequest - the same pattern DeleteUser already follows.
        //
        // This test pins down the current (buggy) behavior so that any future
        // fix is forced to update the assertions here, making the bug
        // impossible to silently re-introduce. Once the controller is fixed
        // to surface failures properly, update this test to assert the new
        // (correct) status code.

        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Result.Fail("User not found"));

        var controller = new UserController(mediatorMock.Object);
        var request = new UpdateUserRequest
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            PhoneNumber = "123-456",
        };

        // Act
        var actionResult = await controller.UpdateUser(Guid.NewGuid(), request);

        // Assert - current buggy behavior: 200 OK carrying a failed Result
        var ok = Assert.IsType<OkObjectResult>(actionResult);
        var payload = Assert.IsType<Result>(ok.Value);
        Assert.True(payload.IsFailed, "Handler returned a failed Result but controller returned 200 OK.");
        Assert.Contains(payload.Errors, e => e.Message == "User not found");
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
