using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Infrastructure.Models;

[Table("password_records")]
public class PasswordRecordModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required] 
    [Column("password")] 
    public string Password { get; set; } = null!;
    
    [Required]
    [Column("changed_at")]
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    [ForeignKey(nameof(UserId))]
    public UsersModel User { get; set; } = null!;
}