using Microsoft.EntityFrameworkCore;
using RSSFeedsAPI.Entities;

namespace RSSFeedsAPI.Data;

public class DataContext : DbContext
{
  public DataContext(DbContextOptions options) : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<Feed>()
        .HasIndex(u => u.Url)
        .IsUnique();
  }

  public DbSet<AppUser> Users { get; set; }
  public DbSet<News> News { get; set; }
}

