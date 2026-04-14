using WareHouse_App.Data;
using WareHouse_App.Entities;

namespace WareHouse_App.Data;

public static class DbSeeder
{
    public static void Seed(WarehouseDbContext context)
    {
        //Warehouses
        if (context.Products.Any()) return;
        var warehouses = new List<Warehouse>
        {
            new Warehouse { Name = "Budapesti Központi Raktár", Location = "Budapest, 11. kerület" },
            new Warehouse { Name = "Győri Logisztikai Depó", Location = "Győr, Ipari park" }
        };
        context.Warehouses.AddRange(warehouses);
        context.SaveChanges();
        
        //Products
        var products = new List<Product>
        {
            new Product { Name = "Logitech MX Master 3S", SKU = "LOG-MX-3S-GR", Price = 35000 },
            new Product { Name = "Dell UltraSharp U2723QE", SKU = "DELL-U2723QE", Price = 210000 },
            new Product { Name = "Keychron K2 V2", SKU = "KEY-K2-V2-RGB", Price = 42000 }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        //InventoryItems)
        var inventoryItems = new List<InventoryItem>
        {
          
            new InventoryItem { ProductId = products[0].Id, WarehouseId = warehouses[0].Id, Quantity = 15 },
            new InventoryItem { ProductId = products[1].Id, WarehouseId = warehouses[0].Id, Quantity = 5 },
            new InventoryItem { ProductId = products[2].Id, WarehouseId = warehouses[0].Id, Quantity = 10 },

           
            new InventoryItem { ProductId = products[0].Id, WarehouseId = warehouses[1].Id, Quantity = 20 },
            new InventoryItem { ProductId = products[2].Id, WarehouseId = warehouses[1].Id, Quantity = 8 }
        };
        context.InventoryItems.AddRange(inventoryItems);

        //Basic Stock Movements
        var movements = new List<StockMovement>
        {
            new StockMovement
            {
                InventoryItemId = inventoryItems[0].Id,
                IsIncoming = true,
                Quantity = 15,
                MovementDate = DateTime.Now.AddDays(-2)

            }
        };
        context.StockMovements.AddRange(movements);

        context.SaveChanges();
    }
}