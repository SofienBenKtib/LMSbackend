using eduflowbackend.Application.Sessions.Delete;
using eduflowbackend.Controllers;
using Moq;
using Xunit;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace eduflowbackend.UnitTests.Controllers;

public class SessionControllerTests
{
    [Fact]
    public async Task DeleteSessionById_WhenSessionExists_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sessionId = Guid.NewGuid();

        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSessionCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        var controller = new SessionController(mediatorMock.Object);

        // Act
        var result = await controller.DeleteSessionById(sessionId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteSessionById_WhenSessionDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sessionId = Guid.NewGuid();

        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSessionCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(false);

        var controller = new SessionController(mediatorMock.Object);

        // Act
        var result = await controller.DeleteSessionById(sessionId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteSessionById_SendsCorrectCommandId()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var sessionId = Guid.NewGuid();

        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSessionCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        var controller = new SessionController(mediatorMock.Object);

        // Act
        await controller.DeleteSessionById(sessionId);

        // Assert - verify the command was sent with the correct ID
        mediatorMock.Verify(
            m => m.Send(
                It.Is<DeleteSessionCommand>(cmd => cmd.Id == sessionId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
