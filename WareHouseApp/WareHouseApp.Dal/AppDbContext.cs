using Microsoft.EntityFrameworkCore;
using WareHouse_App.Entities;

namespace WareHouseApp.Dal;

public class AppDbContext : DbContext
{

    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ag0h6e;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Budapesti Központi Raktár", Location = "Budapest, 11. kerület" },
                new Warehouse { Id = 2, Name = "Győri Logisztikai Depó", Location = "Győr, Ipari park" }
            );
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Logitech MX Master 3S", SKU = "LOG-MX-3S-GR", Price = 35000 },
            new Product { Id = 2, Name = "Dell UltraSharp U2723QE", SKU = "DELL-U2723QE", Price = 210000 },
            new Product { Id = 3, Name = "Keychron K2 V2", SKU = "KEY-K2-V2-RGB", Price = 42000 }
        );
        
        modelBuilder.Entity<InventoryItem>().HasData(
            // Budapest (Id: 1)
            new InventoryItem { Id = 1, ProductId = 1, WarehouseId = 1, Quantity = 15 },
            new InventoryItem { Id = 2, ProductId = 2, WarehouseId = 1, Quantity = 5 },
            new InventoryItem { Id = 3, ProductId = 3, WarehouseId = 1, Quantity = 10 },

            // Győr (Id: 2)
            new InventoryItem { Id = 4, ProductId = 1, WarehouseId = 2, Quantity = 20 },
            new InventoryItem { Id = 5, ProductId = 3, WarehouseId = 2, Quantity = 8 }
        );
        modelBuilder.Entity<StockMovement>().HasData(
            new StockMovement
            {
                Id = 1,
                InventoryItemId = 1, 
                Quantity = 15,
                MovementDate = new DateTime(2026, 4, 26, 10, 0, 0), 
                IsIncoming = true
            }
        );


    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

}
