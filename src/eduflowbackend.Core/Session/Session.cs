using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eduflowbackend.Core.entities;

public class Session
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] [MaxLength(50)] public string Title { get; set; }
    [Required] [MaxLength(50)] public string Description { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }
    
    //  Foreign key for the instructor who created the session
    public Guid InstructorId { get; set; }
    
    //  Navigation properties
    [ForeignKey("InstructorId")]
    public User Instructor { get; set; } // The instructor who created the session
    public ICollection<User> Participants { get; set; }
    public ICollection<Resource> Resources { get; set; }
}