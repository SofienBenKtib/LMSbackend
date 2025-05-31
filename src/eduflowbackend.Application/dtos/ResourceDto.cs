namespace eduflowbackend.Application.dtos;

public class ResourceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid AddedById { get; set; }
    public Guid SessionId { get; set; }
}