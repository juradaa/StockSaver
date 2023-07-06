using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRO_APBD.Server.Models;

[PrimaryKey(nameof(IdUser), nameof(IdStock))]
public class UserStock
{
    public int IdUser { get; set; }
    public int IdStock { get; set; }
    [ForeignKey(nameof(IdUser))]
    public virtual User User { get; set; }
    [ForeignKey(nameof(IdStock))]
    public virtual Stock Stock { get; set; }
}
