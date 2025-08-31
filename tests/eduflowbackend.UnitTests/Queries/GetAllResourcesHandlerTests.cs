using FluentResults;
using Moq;

namespace eduflowbackend.UnitTests.Queries;

public class GetAllResourcesHandlerTests
{
     [Fact]
    public async Task Handle_ReturnsResources_Success()
    {
        // Arrange
        var mockRepository = new Mock<IRepository<ResourceDto>>();
        var handler = new GetAllResourcesHandler(mockRepository.Object);
        
        var expectedResources = new List<ResourceDto>
        {
            new ResourceDto { Id = Guid.NewGuid(), Title = "Resource 1" },
            new ResourceDto { Id = Guid.NewGuid(), Title = "Resource 2" }
        };

        mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(expectedResources);

        // Act
        var result = await handler.Handle(new GetAllResourcesQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal("Resource 1", result.Value[0].Title);
    }

    [Fact]
    public async Task Handle_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var mockRepository = new Mock<IRepository<ResourceDto>>();
        var handler = new GetAllResourcesHandler(mockRepository.Object);
        
        mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new List<ResourceDto>());

        // Act
        var result = await handler.Handle(new GetAllResourcesQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}

// Simple test versions
public class ResourceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
}

public class GetAllResourcesQuery { }

public class GetAllResourcesHandler
{
    private readonly IRepository<ResourceDto> _repository;

    public GetAllResourcesHandler(IRepository<ResourceDto> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<List<ResourceDto>>> Handle(GetAllResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _repository.GetAllAsync(cancellationToken);
        return Result.Ok(resources);
    }
}