using Microsoft.EntityFrameworkCore;
using PwtApi.Models;

namespace PwtApi.Data;

public class PwtDbContext : DbContext
{
    public PwtDbContext(DbContextOptions<PwtDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .ToTable("Varer")
            .HasKey(p => p.EAN);

        modelBuilder.Entity<Inventory>()
            .ToTable("Beholdning")
            .HasKey(i => new { i.ShopID, i.EAN });
    }
} 