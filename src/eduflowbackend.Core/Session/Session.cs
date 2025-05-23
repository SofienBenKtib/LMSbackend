namespace eduflowbackend.Core.Session;

public class Session
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }
    
    public Guid InstructorId { get; set; }
    public User.User Instructor { get; set; } // The instructor who created the session
    public ICollection<User.Participant> Participants { get; set; }
    // public ICollection<Resource.Resource> Resources { get; set; }
}