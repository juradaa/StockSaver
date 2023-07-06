using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRO_APBD.Server.Models;

public class News
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(300)]
    public string PublisherName { get; set; }
    [Required, MaxLength(300)]
    public string Title { get; set; }
    [Required]
    public DateTime PublishDate { get; set; }
    [Required, MaxLength(300)]
    public string AritcleUrl { get; set; }
    public int IdStock { get; set; }
    [ForeignKey(nameof(IdStock))]
    public virtual Stock Stock { get; set; }

}

