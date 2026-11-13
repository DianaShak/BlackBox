using UserService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Models;

[Table("users")]
public class UsersModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(30)]
    [Column("login")]
    public string Login { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; } = null!;
	
    [Required]
    [MaxLength(30)]
    [Column("display_name")]
    public string DisplayName { get; set; }
    
    [MaxLength(10)] 
	[Column("status")]
	public UserStatusEnum Status { get; set; } 
    
    [Required]
    [Column("user_role")]
    public UserRole UserRole { get; set; }
    
    public UserProfilesModel Profile { get; private set; } = null!;

    public List<PasswordRecordModel> PasswordHistory { get; set; } = null!;
}