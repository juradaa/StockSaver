using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_APBD.Shared.DTOs;

public class UserRegistrationDto
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string RepeatPassword { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
}
