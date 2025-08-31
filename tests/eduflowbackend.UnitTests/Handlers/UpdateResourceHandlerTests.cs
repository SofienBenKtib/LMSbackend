using eduflowbackend.Application.Resources.Update;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using Moq;

namespace eduflowbackend.UnitTests.Handlers;

public class UpdateResourceHandlerTests
{
    [Fact]
    public async Task Handle_ResourceExists_UpdatesSuccessfully()
    {
        // Arrange
        var mockRepository = new Mock<IRepository<Resource>>();
        var handler = new UpdateResourceHandler(mockRepository.Object);
        
        var resourceId= Guid.NewGuid();
        var existingResource = new Resource
        {
            Id = resourceId,
            Title = "Old Title",
            Description = "Old Description"
        };
        var command = new UpdateResourceCommand
        {
            Id = resourceId,
            Title = "New Title",
            Description = "New Description"
        };
        // Setup mock to return our test resource
        mockRepository.Setup(r => r.GetByIdAsync(resourceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingResource);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess); // Should succeed
        Assert.Equal("New Title", existingResource.Title); // Title updated
        Assert.Equal("New Description", existingResource.Description); // Description updated
    }
    [Fact]
    public async Task Handle_ResourceNotFound_ReturnsFailure()
    {
        // Arrange
        var mockRepository = new Mock<IRepository<Resource>>();
        var handler = new UpdateResourceHandler(mockRepository.Object);
        
        var command = new UpdateResourceCommand
        {
            Id = Guid.NewGuid(),
            Title = "New Title",
            Description = "New Description"
        };

        // Setup mock to return null (resource not found)
        mockRepository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Resource?)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed); // Should fail
        Assert.Contains("Resource not found", result.Errors[0].Message); // Correct error message
    }
}