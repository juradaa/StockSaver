using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRO_APBD.Server.Models;

public class Hloc

{
    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public Double Open { get; set; }
    [Required]
    public Double High { get; set; }
    [Required]
    public Double Low { get; set; }
    [Required]
    public Double Close { get; set; }
    [Required]
    public Double Volume { get; set; }
    public int IdStock { get; set; }
    [ForeignKey(nameof(IdStock))]
    public virtual Stock Stock { get; set; }

}
