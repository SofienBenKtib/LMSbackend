using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eduflowbackend.Core.entities;

public class Resource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] [MaxLength(50)] public string Title { get; set; }
    [Required] [MaxLength(50)] public string Description { get; set; }
    [Required] public string CreatedBy { get; set; }
    [Required] public DateTime CreatedAt { get; set; }

    //  Foreign keys
    public Guid AddedById { get; set; } //  The Id of the user who created the session
    public Guid SessionId { get; set; } //  The Id of the session where the resource belongs

    //  Navigation Properties
    [ForeignKey("AddedById")] public User AddedBy { get; set; }
    [ForeignKey("SessionId")] public Session Session { get; set; }
}