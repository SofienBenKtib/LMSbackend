using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eduflowbackend.Core.enums;

namespace eduflowbackend.Core.entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] [StringLength(50)] public string FirstName { get; set; }
    [Required] [StringLength(50)] public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; }

    [Required] [StringLength(50)] public string Password { get; set; }
    [Required] [StringLength(50)] public string Phone { get; set; }
    [Required] public Role Role { get; set; }
    [Required] public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    // Navigation properties
    public ICollection<Session> CreatedSessions { get; set; } //Sessions created (If Instructor)
    public ICollection<Session> AttendedSessions { get; set; } //Sessions joined (If Participant)
    public ICollection<Resource> AddedResources { get; set; } //Resources added by the User
}