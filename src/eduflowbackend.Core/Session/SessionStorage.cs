using System.Collections.Concurrent;

namespace eduflowbackend.Core.Session;

public class SessionStorage
{
    public static readonly ConcurrentDictionary<Guid, Session> Sessions = new();
}