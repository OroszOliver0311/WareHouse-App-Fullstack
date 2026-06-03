using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;

namespace WareHouseApp.Dal;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        configurationBuilder.Properties<string>().HaveMaxLength(255);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Budapesti Központi Raktár", Location = "Budapest, 11. kerület" },
                new Warehouse { Id = 2, Name = "Győri Logisztikai Depó", Location = "Győr, Ipari park" }
            );
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Logitech MX Master 3S", SKU = "LOG-MX-3S-GR", UnitPrice = 35000 },
            new Product { Id = 2, Name = "Dell UltraSharp U2723QE", SKU = "DELL-U2723QE", UnitPrice = 210000 },
            new Product { Id = 3, Name = "Keychron K2 V2", SKU = "KEY-K2-V2-RGB", UnitPrice = 42000 }
        );

       
        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => new { i.ProductId, i.WareHouseId })
            .IsUnique();
        modelBuilder.Entity<InventoryItem>().HasData(
            // Budapest (Id: 1)
            new InventoryItem { Id = 1, ProductId = 1, WareHouseId = 1, Quantity = 15 },
            new InventoryItem { Id = 2, ProductId = 2, WareHouseId = 1, Quantity = 5 },
            new InventoryItem { Id = 3, ProductId = 3, WareHouseId = 1, Quantity = 10 },

            // Győr (Id: 2)
            new InventoryItem { Id = 4, ProductId = 1, WareHouseId = 2, Quantity = 20 },
            new InventoryItem { Id = 5, ProductId = 3, WareHouseId = 2, Quantity = 8 }
        );
        modelBuilder.Entity<StockMovement>().HasData(
            new StockMovement
            {
                Id = 1,
                InventoryItemId = 1, 
                Quantity = 15,
                MovementDate = new DateTimeOffset(2026, 4, 26, 10, 0, 0, TimeSpan.Zero), 
                IsIncoming = true
            }
        );
    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

}
