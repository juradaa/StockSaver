using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRO_APBD.Server.Models;

[Table("User")]
public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    public virtual ICollection<UserStock> UserStocks { get; set; }
}
