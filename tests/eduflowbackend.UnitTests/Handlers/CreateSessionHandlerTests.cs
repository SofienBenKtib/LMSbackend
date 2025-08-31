using System.Collections.Concurrent;

namespace eduflowbackend.UnitTests.Handlers;

public class CreateSessionHandlerTests
{
    public class Session
    {
        public Guid Id { get; set; }
        public string Link { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
    }

    public class CreateSessionCommand
    {
        public string Link { get; set; } = string.Empty;
    }

    public static class SessionStorage
    {
        public static ConcurrentDictionary<Guid, Session> Sessions = new ConcurrentDictionary<Guid, Session>();
    }

    public class CreateSessionHandler
    {
        public ValueTask<Session> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Link))
            {
                throw new ArgumentException("Link cannot be null or empty");
            }

            var session = new Session
            {
                Id = Guid.NewGuid(),
                Link = request.Link.Trim(),
                StartDate = DateTime.UtcNow
            };

            SessionStorage.Sessions.TryAdd(session.Id, session);
            return ValueTask.FromResult(session);
        }
    }

    [Fact]
    public void Handle_ValidLink_CreatesSessionSuccessfully()
    {
        // Arrange
        var handler = new CreateSessionHandler();
        var command = new CreateSessionCommand { Link = "https://meet.jit.si/test-room" };
        
        // Clear any previous sessions
        SessionStorage.Sessions.Clear();

        // Act
        var result = handler.Handle(command, CancellationToken.None).Result;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://meet.jit.si/test-room", result.Link);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.True(SessionStorage.Sessions.ContainsKey(result.Id)); // Session was stored
    }

    [Fact]
    public void Handle_EmptyLink_ThrowsArgumentException()
    {
        // Arrange
        var handler = new CreateSessionHandler();
        var command = new CreateSessionCommand { Link = "" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            handler.Handle(command, CancellationToken.None).Result);
    }

    [Fact]
    public void Handle_WhitespaceLink_ThrowsArgumentException()
    {
        // Arrange
        var handler = new CreateSessionHandler();
        var command = new CreateSessionCommand { Link = "   " };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            handler.Handle(command, CancellationToken.None).Result);
    }

    [Fact]
    public void Handle_LinkWithSpaces_TrimsSpaces()
    {
        // Arrange
        var handler = new CreateSessionHandler();
        var command = new CreateSessionCommand { Link = "  https://meet.jit.si/test-room  " };
        
        // Clear any previous sessions
        SessionStorage.Sessions.Clear();

        // Act
        var result = handler.Handle(command, CancellationToken.None).Result;

        // Assert
        Assert.Equal("https://meet.jit.si/test-room", result.Link); // Should be trimmed
    }
}