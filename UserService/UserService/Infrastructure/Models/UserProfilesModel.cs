using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Infrastructure.Models;

[Table("user_profiles")]
public class UserProfilesModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [MaxLength(50)]
    [Column("bio")]
    public string? Bio { get; set; }
    
    [Column("birth_date")]
    public DateOnly? BirthDate { get; set; }

    [Required]
	[Column("created_at")]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [ForeignKey(nameof(UserId))]
    public UsersModel User { get; set; } = null!;
}