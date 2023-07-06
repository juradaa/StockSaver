using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace PRO_APBD.Server.Models;

public class Stock
{
    [Key]
    public int Id { get; set; }
    [Required,MaxLength(300)]
    public string Ticker { get; set; }
    [Required,MaxLength(300)]
    public string Name { get; set; }
    [Required,MaxLength(300)]
    public string ImageUrl { get; set; }
    [Required,MaxLength(300)]
    public string Country { get; set; }
    [Required,MaxLength(300)]
    public string HomepageUrl { get; set; }
    [Required,MaxLength(300)]
    public string Currency { get; set; }
    public virtual ICollection<UserStock> UserStocks { get; set; }
    public virtual ICollection<News> News { get; set; }
    public virtual SessionEnd SessionEnd { get; set; }
    public virtual ICollection<Hloc> Hlocs { get; set; }
}
