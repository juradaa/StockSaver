using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRO_APBD.Server.Models;

public class SessionEnd
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(255)]
    public string From { get; set; }
    [Required]
    public double Open { get; set; }
    [Required]
    public double High { get; set; }
    [Required]
    public double Low { get; set; }
    [Required]
    public double Close { get; set; }
    [Required]
    public double Volume { get; set; }
    [Required]
    public double AfterHours { get; set; }
    [Required]
    public double PreMarket { get; set; }
    public int IdStock { get; set; }
    [ForeignKey(nameof(IdStock))]
    public virtual Stock Stock { get; set; }
}
