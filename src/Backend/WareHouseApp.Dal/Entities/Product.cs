using Microsoft.EntityFrameworkCore;

namespace WareHouse_App.Entities;

[Index(nameof(SKU), IsUnique = true)]
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public required decimal UnitPrice { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; } = [];

}
