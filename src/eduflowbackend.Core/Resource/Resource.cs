using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eduflowbackend.Core.Resource;

public class Resource : AuditableEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CreatorId { get; set; } //  The Id of the user who created the session
    public User.User Creator { get; set; }
    public Guid SessionId { get; set; } //  The Id of the session where the resource belongs
    public Session.Session Session { get; set; }

    public Resource()
    {
        
    }

    public Resource(string title, string description)
    {
        
    }
}