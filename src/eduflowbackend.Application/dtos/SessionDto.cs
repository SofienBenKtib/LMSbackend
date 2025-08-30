namespace eduflowbackend.Application.dtos;

public class SessionDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public required string Link { get; set; }
    /*public string Description { get; set; }*/
    public DateTime StartDate { get; set; } = DateTime.Now;
    /*public DateTime EndDate { get; set; }*/
    // Instructor info
    public Guid InstructorId { get; set; }
}