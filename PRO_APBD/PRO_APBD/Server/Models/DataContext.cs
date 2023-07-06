using Microsoft.EntityFrameworkCore;

namespace PRO_APBD.Server.Models;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Hloc> Hlocs { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<SessionEnd> SessionEnds { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<UserStock> UserStocks { get; set; }
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
