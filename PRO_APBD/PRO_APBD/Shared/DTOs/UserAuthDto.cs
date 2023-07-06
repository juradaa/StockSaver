using System.ComponentModel.DataAnnotations;

namespace PRO_APBD.Shared.DTOs
{
    public class UserAuthDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
