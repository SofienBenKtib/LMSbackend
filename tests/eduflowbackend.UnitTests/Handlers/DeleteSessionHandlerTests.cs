using System.Collections.Concurrent;

namespace eduflowbackend.UnitTests.Handlers;

public class DeleteSessionHandlerTests
{
    public class Session
    {
        public Guid Id { get; set; }
        public string Link { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
    }

    public static class SessionStorage
    {
        public static ConcurrentDictionary<Guid, Session> Sessions = new ConcurrentDictionary<Guid, Session>();
    }

    public record DeleteSessionCommand(Guid Id);

    public class DeleteSessionHandler
    {
        public ValueTask<bool> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(SessionStorage.Sessions.TryRemove(request.Id, out _));
        }
    }

    [Fact]
    public void Handle_ExistingSession_ReturnsTrueAndRemovesSession()
    {
        // Arrange
        var handler = new DeleteSessionHandler();
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            Link = "https://meet.jit.si/test-room",
            StartDate = DateTime.UtcNow
        };

        SessionStorage.Sessions.Clear();
        SessionStorage.Sessions.TryAdd(sessionId, session);

        // Act
        var result = handler.Handle(new DeleteSessionCommand(sessionId), CancellationToken.None).Result;

        // Assert
        Assert.True(result);
        Assert.False(SessionStorage.Sessions.ContainsKey(sessionId));
    }

    [Fact]
    public void Handle_NonExistingSession_ReturnsFalse()
    {
        // Arrange
        var handler = new DeleteSessionHandler();
        var nonExistentId = Guid.NewGuid();

        SessionStorage.Sessions.Clear();

        // Act
        var result = handler.Handle(new DeleteSessionCommand(nonExistentId), CancellationToken.None).Result;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Handle_DeleteSameSessionTwice_SecondDeleteReturnsFalse()
    {
        // Arrange
        var handler = new DeleteSessionHandler();
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            Link = "https://meet.jit.si/test-room",
            StartDate = DateTime.UtcNow
        };

        SessionStorage.Sessions.Clear();
        SessionStorage.Sessions.TryAdd(sessionId, session);

        // Act - first delete should succeed
        var firstResult = handler.Handle(new DeleteSessionCommand(sessionId), CancellationToken.None).Result;
        // Act - second delete of the same session should fail
        var secondResult = handler.Handle(new DeleteSessionCommand(sessionId), CancellationToken.None).Result;

        // Assert
        Assert.True(firstResult);
        Assert.False(secondResult);
        Assert.False(SessionStorage.Sessions.ContainsKey(sessionId));
    }

    [Fact]
    public void Handle_DeleteSession_DoesNotAffectOtherSessions()
    {
        // Arrange
        var handler = new DeleteSessionHandler();
        var sessionIdToDelete = Guid.NewGuid();
        var sessionIdToKeep = Guid.NewGuid();

        SessionStorage.Sessions.Clear();
        SessionStorage.Sessions.TryAdd(sessionIdToDelete, new Session
        {
            Id = sessionIdToDelete,
            Link = "https://meet.jit.si/room-to-delete",
            StartDate = DateTime.UtcNow
        });
        SessionStorage.Sessions.TryAdd(sessionIdToKeep, new Session
        {
            Id = sessionIdToKeep,
            Link = "https://meet.jit.si/room-to-keep",
            StartDate = DateTime.UtcNow
        });

        // Act
        var result = handler.Handle(new DeleteSessionCommand(sessionIdToDelete), CancellationToken.None).Result;

        // Assert
        Assert.True(result);
        Assert.False(SessionStorage.Sessions.ContainsKey(sessionIdToDelete));
        Assert.True(SessionStorage.Sessions.ContainsKey(sessionIdToKeep));
        Assert.Single(SessionStorage.Sessions);
    }

    [Fact]
    public void Handle_DeleteWithEmptyGuid_ReturnsFalse()
    {
        // Arrange
        var handler = new DeleteSessionHandler();

        SessionStorage.Sessions.Clear();

        // Act
        var result = handler.Handle(new DeleteSessionCommand(Guid.Empty), CancellationToken.None).Result;

        // Assert
        Assert.False(result);
    }
}
